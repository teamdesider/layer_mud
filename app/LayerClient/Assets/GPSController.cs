using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class GPSController : MonoBehaviour
{
    private Vector3 worldGpsOrigin; // GPS 
    private Vector3 gpsOrigin;
    private bool originSet = false;
    public Camera arCamera;
    public bool isRequesting = false;
    private float _LatOrigin { get { return gpsOrigin.x; } }
    private float _LonOrigin { get { return gpsOrigin.z; } }
    private float metersPerLat;
    private float metersPerLon;
    private void Start()
    {
        //DebugManager.Instance.LogInfo("GPS is starting ");

        //GetGPSLocation();
    }

    private void Update()
    {
        //GetGPSLocation();
    }

    public void GetGPSLocation()
    {
        if (!isRequesting)
        {
            StartCoroutine(StartGPSLocationCoroutine());
            isRequesting = !isRequesting;
        }
    }

    public async Task<List<float>> GetGPSLocationAsync()
    {
        var res =new List<float>();
        if (!isRequesting)
        {
            res= await GetValFromGPS();
        }

        return res;
    }
    private async Task<List<float>> GetValFromGPS()
    {
        isRequesting = true;
        int maxWait = 3;
        if (!Input.location.isEnabledByUser)
        {
            await Task.Delay(maxWait);
        }

        Input.location.Start();

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            await Task.Delay(1);

            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("GPS init timeout");
            DebugManager.Instance.LogError("GPS init timeout");

            return new List<float>();
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            DebugManager.Instance.LogError("Unable to determine location");
        }
        else
        {
            if (!originSet)
            {
                gpsOrigin = new Vector3(Input.location.lastData.latitude, 0f, Input.location.lastData.longitude);
                worldGpsOrigin = arCamera.transform.position;
                DebugManager.Instance.LogInfo("Current Lat: " + Input.location.lastData.latitude + " long: " + Input.location.lastData.longitude);
                originSet = true;
                //FindMetersPerLat(_LatOrigin);
            }

        }

        isRequesting = false;
        return new List<float> { Input.location.lastData.latitude, Input.location.lastData.longitude };

    }

    private IEnumerator StartGPSLocationCoroutine()
    {
        int maxWait = 3;
        if (!Input.location.isEnabledByUser)
        {
            yield return new WaitForSeconds(maxWait);
        }

        Input.location.Start();

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("GPS init timeout");
            DebugManager.Instance.LogError("GPS init timeout");

            yield break;
        }

        DebugManager.Instance.LogInfo("RUNNING");


        if (Input.location.status == LocationServiceStatus.Failed)
        {
            DebugManager.Instance.LogError("Unable to determine location");
            Debug.Log("Unable to determine location");

        }
        else
        {
            if (!originSet)
            {
                gpsOrigin = new Vector3(Input.location.lastData.latitude, 0f, Input.location.lastData.longitude);
                worldGpsOrigin = arCamera.transform.position;
                DebugManager.Instance.LogInfo("Current Lat: "+ Input.location.lastData.latitude +" long: "+ Input.location.lastData.longitude);
                originSet = true;
                //FindMetersPerLat(_LatOrigin);
            }
            else
            {
                //var gpsData = new Vector3(Input.location.lastData.latitude, 0f, Input.location.lastData.longitude);
                //ConvertGPStoUCS(gpsData);
            }

        }

        isRequesting = !isRequesting;

    }

    //public void FindMetersPerLat(float lat) // Compute lengths of degrees
    //{
    //    float m1 = 111132.92f;    // latitude calculation term 1
    //    float m2 = -559.82f;        // latitude calculation term 2
    //    float m3 = 1.175f;      // latitude calculation term 3
    //    float m4 = -0.0023f;        // latitude calculation term 4
    //    float p1 = 111412.84f;    // longitude calculation term 1
    //    float p2 = -93.5f;      // longitude calculation term 2
    //    float p3 = 0.118f;      // longitude calculation term 3

    //    lat *= Mathf.Deg2Rad;

    //    // Calculate the length of a degree of latitude and longitude in meters
    //    metersPerLat = m1 + (m2 * Mathf.Cos(2 * (float)lat)) + (m3 * Mathf.Cos(4 * (float)lat)) + (m4 * Mathf.Cos(6 * (float)lat));
    //    metersPerLon = (p1 * Mathf.Cos((float)lat)) + (p2 * Mathf.Cos(3 * (float)lat)) + (p3 * Mathf.Cos(5 * (float)lat));
    //}

    //private Vector3 ConvertGPStoUCS(Vector3 gps)
    //{
    //    //FindMetersPerLat(_LatOrigin);
    //    float xPosition = worldGpsOrigin.x + metersPerLat * (gps.x - _LatOrigin);
    //    float zPosition = worldGpsOrigin.z + metersPerLon * (gps.z - _LonOrigin);
    //    return new Vector3(xPosition, 0, zPosition);
    //}


}