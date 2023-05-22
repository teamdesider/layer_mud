using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public Inventory inventory = new();
    public InventoryUIController inventoryUIController;
    public BagSliderIndicatorController bagSliderIndicatorController;
    private readonly int defaultAmount = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool InitInventory()
    {

        inventory.InitInventory();
        inventoryUIController.InitPageGB(inventory);
        bagSliderIndicatorController.InitSlideIndicator(inventory.inventoryList.Count);

        return true;
    }

    public void ModifyInventory()
    {
        inventory.ModifyItem("item1", -defaultAmount);
    }

    public void UpdateInventory(string itemTypeId, InventoryActions action)
    {
        switch (action)
        {
            case InventoryActions.delete:
                break;
            default:
                if (inventory.inventoryDict.ContainsKey(itemTypeId))
                {
                    inventory.ModifyItem(itemTypeId, defaultAmount);

                }
                else
                {
                    inventory.CreateItem(itemTypeId, "sdfasd", 1);
                    //inventoryUIController.UpdateUIInventory(inventory.inventoryDict[itemId]);
                }
                inventoryUIController.UpdateUIInventory(inventory.inventoryDict[itemTypeId]);

                return;

        }



    }

    public void GetItems()
    {
        string res = "";
        foreach (KeyValuePair<string, InventoryItem> item in inventory.inventoryDict)
        {
            res += "id:" + item.Value.itemTypeId + "; item Amount:" + item.Value.amount + "-";
        }

        Debug.Log(res);
    }





}


public class Inventory
{

    public Dictionary<string, InventoryItem> inventoryDict = new();
    public List<InventoryItem> inventoryList = new();


    // TODO connect with backend
    public bool AddItem(string itemId, InventoryItem inventoryItem)
    {
        inventoryDict.Add(itemId, inventoryItem);
        //return false;
        return true;
    }

    public bool CreateItem(string itemId, string iconSrc, int amount)
    {
        InventoryItem inventoryItem = new(itemId, amount);
        inventoryDict.Add(itemId, inventoryItem);
        inventoryList.Add(inventoryItem);
        //return false;
        return true;
    }

    // TODO connect with backend
    public bool DeleteItem(string itemId)
    {
        inventoryList.FirstOrDefault(obj => obj.itemTypeId == itemId);
        inventoryDict.Remove(itemId);
        //return false;
        return true;
    }

    public bool ModifyItem(string itemId, int modifyAmount)
    {

        int amount = inventoryDict[itemId].amount;
        if (amount + modifyAmount < 0)
        {
            return false;
        }

        inventoryDict[itemId].amount += modifyAmount;

        if (inventoryDict[itemId].amount == 0)
        {
            DeleteItem(itemId);
        }

        return true;
    }

    public bool InitInventory()
    {
        List<InventoryItem> temp = new()
        {
            //new InventoryItem("cub_1", 5),
            //new InventoryItem("cub_2", 4),
            //new InventoryItem("lucium_1", 2),
            //new InventoryItem("seed_3", 1)
        };
        int idx = 0;
        foreach (var item in temp)
        {
            inventoryDict.Add(item.itemTypeId, item);

            inventoryList.Add(item);
            idx++;
        }



        return true;
    }

    //public bool updateInventory()
    //{
    //            List<InventoryItem> temp = new()
    //    {
    //        new InventoryItem("item1", "/sidafjosiafsd", 5),
    //        new InventoryItem("item2", "/sidafjosiafsd", 4),
    //        new InventoryItem("item3", "/sidafjosiafsd", 3),
    //        new InventoryItem("item4", "/sidafjosiafsd", 2),
    //        new InventoryItem("item5", "/sidafjosiafsd", 1)
    //    };
    //    int idx = 0;
    //    foreach (var item in temp)
    //    {
    //        inventoryDict.Add(item.itemId, item);

    //        inventoryList.Add(item);
    //        idx++;
    //    }

    //    return true;
    //}
}
public class InventoryItem
{
    public string itemTypeId;
    private int _amount = -1;
    public int amount
    {
        get
        {
            return _amount;
        }
        set
        {


            if (this._amount == -1)
            {
                _amount = value;
            }
            else
            {
                if (this._amount != value)
                {
                    _amount = value;
                    //InventoryUIController.UpdateUIInventory(this);
                }
            }





        }
    }

    public InventoryItem(string itemTypeId, int amount)
    {
        this.itemTypeId = itemTypeId;
        this.amount = amount;
    }
}
