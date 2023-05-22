using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameStateController : MonoBehaviour
{

    public PlayerStateController playerStateController;
    public InventoryController inventoryController;
    public Camera arCamera;
    //public CrosshairController crosshairController;
    public RequestRemoteController requestRemoteController;
    public CompassController compassController;
    public GPSController gPSController;
    private GameObject itemsWrapper = null;
    private LocationServiceManager locationServiceManager;
    private int viewScope = 5;
    private ConcurrentDictionary<string, PendingItem> pendingDict = new();
    private readonly int collectSpendEnergy = 1;
    //TODO test for 10 second
    private float checkStatusDuration = 6f;
    private float timer = 0f;

    private int num = 10;
    // Start is called before the first frame update
    async void Start()
    {
        playerStateController = GetComponent<PlayerStateController>();
        inventoryController = GetComponent<InventoryController>();
        //crosshairController = GetComponent<CrosshairController>();
        requestRemoteController = GetComponent<RequestRemoteController>();
        compassController = GetComponent<CompassController>();
        gPSController = GetComponent<GPSController>();
        locationServiceManager = GetComponent<LocationServiceManager>();
        Debug.Log("Init game state " + await InitGameState());
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= checkStatusDuration && pendingDict.Keys.Count>0)
        {
            CheckPendingStatus();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }



     public async Task<bool> InitGameState()
    {
        playerStateController.InitPlayerState();
        inventoryController.InitInventory();
        itemsWrapper = new GameObject();

        itemsWrapper.transform.position = arCamera.transform.position;
        //TODO GetGPS
        //TODO wati to request
        var gpsVals = await gPSController.GetGPSLocationAsync();

        if (gpsVals.Count != 2)
        {
            return false;
        }
        await requestRemoteController.RequestCollectableItems(gpsVals[0], gpsVals[1]);
        GenerateItems(requestRemoteController.insLst.data);
        while(!locationServiceManager.locationServiceEnabled){
            await Task.Yield();
        }

        //DebugManager.Instance.LogInfo("After " + locationServiceManager.locationServiceEnabled.ToString());


        var rotation = compassController.GetCurrentNorthRotation(itemsWrapper);
        Vector3 rot = rotation.eulerAngles;
        //DebugManager.Instance.LogInfo("Rotation eul " + rot.x + " " + rot.y + " " + rot.z);
        itemsWrapper.transform.rotation = rotation;
        //Debug.Log(playerStateController.playerState);
        //Debug.Log(inventoryController.inventory.inventoryDict.Count);

        return true;
        //return false;
    }

    private void GenerateItems(List<RemoteInstance> lst)
    {
        foreach (RemoteInstance remoteIns in lst)
        {
            // remote.id is not same meaning to GameItem id, type is corresponding to gameItem id
            GameItem gameObjeceIns = ItemDictionary.itemsDict[((RemoteToGameConnect)remoteIns.type).ToString()];
            GameObject ins = Instantiate(gameObjeceIns.prefab);
            ItemAttributeScript attributes = ins.AddComponent<ItemAttributeScript>();
            attributes.itemId = remoteIns.id;
            attributes.itemName = gameObjeceIns.itemName;
            ins.tag = gameObjeceIns.tag;
            attributes.itemTypeId = gameObjeceIns.itemId;
            ins.transform.SetParent(itemsWrapper.transform);
            ins.transform.SetLocalPositionAndRotation(new Vector3(remoteIns.GetX(), 0, remoteIns.GetZ()), Quaternion.identity);
        }
    }

    public bool UpdateEnergy(int modVal)
    {
        playerStateController.ModifyEnergy(modVal);
        return true;
        return false;
    }

    public bool UpdateToken(int modVal)
    {
        playerStateController.ModifyCoin(modVal);
        return true;
    }

    public bool UpdateInventory(string itemTypeId, InventoryActions action)
    {

        inventoryController.UpdateInventory(itemTypeId, action);
        return true;
    }

    public void Addinventory()
    {
        inventoryController.UpdateInventory("item" + num, InventoryActions.collect);
        num++;
    }

    public void CheckInventory()
    {
        inventoryController.GetItems();
    }

    public void AddNewPending(GameObject gb)
    {
        string itemId = gb.GetComponent<ItemAttributeScript>().itemId;
        pendingDict.TryAdd(itemId, new PendingItem() { gameobject=gb});
        requestRemoteController.SendCollectedItem(itemId);
    }

    private void CheckPendingStatus()
    {
        foreach (KeyValuePair<string, PendingItem> pair in pendingDict)
        {
            //Debug.Log("List item: " + pair.Key + " status:" + pair.Value.status);
            string itemId = pair.Value.gameobject.GetComponent<ItemAttributeScript>().itemId;
            string itemName = pair.Value.gameobject.GetComponent<ItemAttributeScript>().itemName;

            switch (pair.Value.status)
            {
                case PendingItemStatus.pending:
                    DebugManager.Instance.LogWarning("ItemName: "+itemName +" is pending.");
                    requestRemoteController.CheckCollectableOnChainStatus(pair.Key, pendingDict);
                    break;
                case PendingItemStatus.settled:
                    DebugManager.Instance.LogSuccess("ItemName: "+ itemName + " is confirmed on chain");
                    UpdateCollectItemsState(pair.Value.gameobject);
                    break;
                case PendingItemStatus.failed:
                    RevertPending(pair.Value.gameobject);
                    break;
                case PendingItemStatus.checking:
                    break;
            }

            
        }
    }

    public void UpdateCollectItemsState(GameObject gb)
    {
        string itemTypeId = gb.GetComponent<ItemAttributeScript>().itemTypeId;

        if (gb.CompareTag("Collectable")){
            UpdateInventory(itemTypeId, InventoryActions.collect);
        }else if (gb.CompareTag("Token"))
        {
            UpdateToken(1);
        }
        UpdateEnergy(-collectSpendEnergy);
        CompleteCollect(gb);

    }

    public void CompleteCollect(GameObject gb)
    {
        string itemId = gb.GetComponent<ItemAttributeScript>().itemId;
        pendingDict.TryRemove(itemId, out _);
        Destroy(gb);

    }

    public void RevertPending(GameObject gb)
    {
        gb.SetActive(true);
        CollectableItemController collectItemController = gb.GetComponent<CollectableItemController>();
        string itemId = gb.GetComponent<ItemAttributeScript>().itemId;
        pendingDict.TryRemove(itemId, out _);
        if (collectItemController.isActiveAndEnabled)
        {
            collectItemController.RevertDisappear();
        }
    }
}

public class PendingItem
{
    public GameObject gameobject;
    public PendingItemStatus status = PendingItemStatus.pending;

}

public enum PendingItemStatus
{
    pending,
    checking,
    settled,
    failed
}