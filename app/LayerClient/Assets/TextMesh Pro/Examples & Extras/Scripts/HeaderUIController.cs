using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HeaderUIController : MonoBehaviour
{

    public TMP_Text availableEnergy;
    public TMP_Text maxEnergy;
    public TMP_Text coinVal;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModifyCoin(int modVal) {
        coinVal.text = (modVal).ToString();
    }

    public void ModifyEnergy(int modVal) {
        availableEnergy.text = modVal.ToString();

    }

    public void ModifyMaxEnergy(int changeVal)
    {
        maxEnergy.text = (changeVal).ToString();

    }
}
