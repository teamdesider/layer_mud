using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CompassController : MonoBehaviour
{
    private Compass compass;
    public Camera arCamera;
    public Vector3 northVect;
    public float headingDegree = -1;
    private Vector3 axiaX;
    private Vector3 axiaY;
    private Vector3 axiaZ;
    public TextMeshProUGUI textArea;
    private float timeLimit = 1f;
    private float timeCount = 0f;
    private float maxWait = 3;
    private float timer = 0;
    private LocationServiceManager locationServiceManager;



    void Start()
    {
        //Debug.Log("compass start");
        locationServiceManager = GetComponent<LocationServiceManager>();
        //Debug.Log(locationServiceManager.locationServiceEnabled);
        //DebugManager.Instance.LogInfo("compass start ini");

        Input.location.Start();
        //DebugManager.Instance.LogInfo("compass location start ini");

        //Input.compass.enabled = true;
        //TODO PRODUCTION
        if (!Input.compass.enabled)
        {

            Input.compass.enabled = true;
            //DebugManager.Instance.LogInfo("enabled triggered");

            compass = Input.compass;
            axiaX = arCamera.transform.right;
            axiaY = arCamera.transform.up;
            axiaZ = arCamera.transform.forward;
        }
        else
        {
            DebugManager.Instance.LogError("Please open permission");
            //Debug.Log("Please open permission");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (timeCount >= timeLimit)
        {
            GetCompassVal();

        }
        else
        {
            timeCount += Time.deltaTime;
        }
    }

    public float GetCompassVal()
    {

        //TODO PRODUCTION
        if (compass.enabled)
        {
            headingDegree = compass.trueHeading;

            return headingDegree;
        }

        //DebugManager.Instance.LogError("The compass is not enabled");



        //headingDegree = 80f;
        //return 80f;

        //TODO PRODUCTION
        return -1f;
    }

    public void GetNorthVector()
    {


        if (headingDegree==-1)
        {
            //DebugManager.Instance.LogInfo("Compass is not enable");
            var currentVector = Vector3.ProjectOnPlane(arCamera.transform.forward, Vector3.up).normalized;
            northVect = Quaternion.Euler(0, 0, 0) * currentVector;
        }
        else
        {
            //DebugManager.Instance.LogInfo("Compass is enable");
            var currentVector = Vector3.ProjectOnPlane(arCamera.transform.forward, Vector3.up).normalized;
            northVect = Quaternion.Euler(0, -headingDegree, 0) * currentVector;
        }

    }

    //Get the init GB and northVector quaternion
    public Quaternion GetCurrentNorthRotation(GameObject wrapper)
    {
        Vector3 wrapperAxisZ = wrapper.transform.forward;
        WaitForCompassReading();
        GetNorthVector();
        Vector3 northVect = this.northVect;
        return Quaternion.FromToRotation(wrapperAxisZ, northVect);

    }

    private bool WaitForCompassReading()
    {
        float threshold = 0.5f;

        while (timer <= maxWait)
        {
            if (Input.compass.enabled && Input.compass.trueHeading > threshold)
            {
                headingDegree = Input.compass.trueHeading;
                return true;
            }

            timer += Time.deltaTime;

        }

        timer = 0;

        DebugManager.Instance.LogError("Compass gets val timeout");
        return false;


    }

    //public async Task<bool> WaitLocationServiceEnabled()
    //{
    //    float timeout = 3;
    //    float timer = 0;


    //    while (timer <= timeout)
    //    {
    //        DebugManager.Instance.LogInfo("4.5");

    //        //DebugManager.Instance.LogInfo(locationServiceManager.locationServiceEnabled.ToString());

    //        //DebugManager.Instance.LogInfo("4.6");

    //        Debug.Log(locationServiceManager.locationServiceEnabled);
    //        //if (locationServiceManager.locationServiceEnabled)
    //        //{
    //        //    DebugManager.Instance.LogInfo("5");

    //        //    return true;
    //        //}
    //        //else
    //        //{
    //        //    DebugManager.Instance.LogWarning("5.5");

    //        //}

    //        timer += Time.deltaTime;
    //        DebugManager.Instance.LogWarning((timer).ToString());
    //        await Task.Yield();
    //    }


    //    DebugManager.Instance.LogInfo("locationService dead");

    //    return false;


    //}

}



