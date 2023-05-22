using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FloorDetection : MonoBehaviour
{
    private ARPlaneManager arPlaneManager;
    public GameObject staticGB;
    bool floorFound = false;
    private void Start()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void Update()
    {
        if (!floorFound)
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                if (plane.classification == PlaneClassification.Floor)
                {
                    Vector3 pos = staticGB.transform.position;
                    pos.y  = plane.transform.position.y+0.01f;

                    staticGB.transform.position = pos;
                    floorFound = true;
                }
            }
        }

    }
}
