using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public CrosshairUIController crosshairUIController;
    public bool locked = false;

    // Start is called before the first frame update
    void Start()
    {

        //crosshairUIController.updateLocked(locked);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLocked(bool status)
    {
        if (status != locked)
        {
            locked = status;
            crosshairUIController.updateLocked(locked);
        }

    }
    public Vector3 GetCrossHairScreenPosition()
    {
        return crosshairUIController.GetCrossHairCenterPos();

    }
}
