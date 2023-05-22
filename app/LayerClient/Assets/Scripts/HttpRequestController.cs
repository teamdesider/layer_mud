using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class HttpRequestController : MonoBehaviour
{
    //private void Start()
    //{
    //    SendHttpRequest("https://test-api.desider.com/dwgo/api/layer/collectblock", new CollectBlockBody() { id = "ashfdusf" });
    //}
    public readonly string collectUrl = "https://test-api.desider.com/mud_layer/collect";

    //TODO
    public readonly string requestCollectableItemsUrl = "https://test-api.desider.com/mud_layer/list";

    //TODO
    public readonly string checkCollectStatusUrl = "https://test-api.desider.com/mud_layer/checkcollect";


    public async Task<string> SendHttpRequest(string path, RequestType type = RequestType.GET, object postData = null)
    {
        UnityWebRequest request = new();
        switch (type)
        {
            case RequestType.GET:
                request = CreateRequest(path, RequestType.GET, postData);
                break;
            case RequestType.POST:
                request = CreateRequest(path, RequestType.POST, postData);
                break;
        }
        //var postRequest = CreateRequest(path, RequestType.POST, postData);

        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log(request.downloadHandler.text);
            return request.downloadHandler.text;
        }
        else
        {
            //Debug.LogError(request.downloadHandler.text);
            return "error";
        }

    }

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }


}

public enum RequestType
{
    GET = 0,
    POST = 1,
    PUT = 2,
    DELETE = 3
}

public class CollectBlockBody
{
    public string id;
}

public class CollectResult {
    public int code;
    public string message;
}
