using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class RequestRemoteController : MonoBehaviour
{

    public RemoteList insLst = null;
    private TextAsset jsonFile = null;
    public HttpRequestController httpRequestController;
    // Start is called before the first frame update
    void Start()
    {
        httpRequestController=GetComponent<HttpRequestController>();
        //jsonFile = Resources.Load<TextAsset>("remote");
        ////Debug.Log(jsonFile);
        //string jsonText = jsonFile.text;
        //insLst = JsonUtility.FromJson<RemoteList>(jsonText);

        //foreach(RemoteInstance data in insLst.data)
        //{
        //    Debug.Log("id" + data.id);
        //    Debug.Log("type" + data.type);
        //}
    }



    // Update is called once per frame
    void Update()
    {

    }

    public async void SendCollectedItem(string itemId)
    {

        while (httpRequestController == null)
        {
            await Task.Yield();
        }

        string res = await httpRequestController.SendHttpRequest(httpRequestController.collectUrl, RequestType.POST, new CollectBlockBody() { id = itemId });
    }

    public async void CheckCollectableOnChainStatus(string itemId,ConcurrentDictionary<string, PendingItem> pendingDict) {
        while (httpRequestController == null)
        {
            await Task.Yield();
        }


        pendingDict[itemId].status = PendingItemStatus.checking;


        //TODO Production
        string finalUrl = httpRequestController.checkCollectStatusUrl + "?id=" + itemId;
        //pendingDict[itemId].status = PendingItemStatus.pending;

        string res = await httpRequestController.SendHttpRequest(finalUrl, RequestType.GET);



        //await Task.Delay(3);

        //var status = new StatusInstance() { code = 1 };

        if (res != "error")
        {
            var status = JsonUtility.FromJson<StatusInstance>(res);

            if (status.code == 1)
            {
                pendingDict[itemId].status = PendingItemStatus.settled;
            }
            else
            {
                pendingDict[itemId].status = PendingItemStatus.pending;

            }
        }
        else
        {
            pendingDict[itemId].status = PendingItemStatus.failed;

        }

        //else
        //{
        //    pendingDict[itemId].status = PendingItemStatus.failed;

        //}

    }

    //TODO
    public async Task<bool> RequestCollectableItems(float latitude, float longitude)
    {
        ////TODO
        string latitudeString = latitude.ToString("F6");
        string longitudeString = longitude.ToString("F6");

        string latIntegerValueString = latitudeString.Replace(".", "");
        string lonIntegerValueString = longitudeString.Replace(".", "");

        while (httpRequestController == null)
        {
            await Task.Yield();
        }

        //todo backend log to lon
        string url = httpRequestController.requestCollectableItemsUrl + "?lat="
            + latIntegerValueString + "&" + "log=" + lonIntegerValueString;


        string res = await httpRequestController.SendHttpRequest(url, RequestType.GET);


        if (res == "error")
        {
            // If the backend is down, use the local data to generate
            //Debug.Log("error occur");
            jsonFile = Resources.Load<TextAsset>("remote");
            string jsonText = jsonFile.text;
            insLst = JsonUtility.FromJson<RemoteList>(jsonText);
        }
        else
        {
            //Debug.Log("normal occur");

            insLst = JsonUtility.FromJson<RemoteList>(res);
        }
        DebugManager.Instance.LogSuccess("Request Collectable Item Success");



        return true;


        //return true;



        //foreach (RemoteInstance data in insLst.data)
        //{
        //    Debug.Log("id" + data.id);
        //    Debug.Log("type" + data.type);
        //}
    }

    //private async Task<bool> CheckHttpRequestController() {

    //    while (httpRequestController == null)
    //    {
    //        await Task.Yield();
    //    }

    //    return true;
    //}
}

public class StatusInstance
{
    public int code;
    public string data;
}

[Serializable]
public class RemoteInstance
{
    public string id;
    public int type;
    public List<int> pos = new();

    private int _xIdx = 0;
    private int _zIdx = 1;

    public int GetZ()
    {
        return pos[_zIdx];
    }

    public int GetX()
    {
        return pos[_xIdx];
    }
}



[Serializable]
public class RemoteList
{
    public List<RemoteInstance> data;

}



