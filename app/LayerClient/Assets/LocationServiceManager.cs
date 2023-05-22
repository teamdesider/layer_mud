using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationServiceManager : MonoBehaviour
{
    public bool locationServiceEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitLocationService());
    }

    private IEnumerator InitLocationService()
    {
        int maxWait = 3;
        if (!Input.location.isEnabledByUser)
        {
            DebugManager.Instance.LogError("Please enable location service");
            yield return new WaitForSeconds(maxWait);
        }

        Input.location.Start();

        DebugManager.Instance.LogWarning(Input.location.status.ToString());

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            DebugManager.Instance.LogError("Location service init timeout");

            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            DebugManager.Instance.LogError("Unable to determine location");
            yield break;
        }

        locationServiceEnabled = true;
    }



}
