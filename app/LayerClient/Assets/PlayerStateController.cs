using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{

    public PlayerState playerState = null;
    public HeaderUIController headerUIController = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool InitPlayerState()
    {
        playerState = new PlayerState("testId", "sjfiosdaf", 10, 20);
        headerUIController.ModifyCoin(playerState.coinVal);
        headerUIController.ModifyEnergy(playerState.energy);

        return true;
    }

    public bool ModifyCoin(int modVal)
    {
        int modifiedVal = playerState.coinVal + modVal;
        if (modifiedVal < 0)
        {
            return false;
        }
        else
        {
            playerState.coinVal = modifiedVal;
            headerUIController.ModifyCoin(modifiedVal);

            return true;
        }

    }

    public bool ModifyEnergy(int modVal)
    {
        int modifiedVal = playerState.energy + modVal;
        if (modifiedVal < 0 || modifiedVal > 100)
        {
            return false;
        }
        else
        {
            playerState.energy = modifiedVal;
            headerUIController.ModifyEnergy(modifiedVal);

            return true;
        }
    }
}

public class PlayerState
{
    public string playerId;
    public string playerIcon;
    public int coinVal;
    public int energy;

    public PlayerState(string playerId, string playerIcon, int coinVal, int energy)
    {
        this.playerIcon = playerIcon;
        this.playerId = playerId;
        this.coinVal = coinVal;
        this.energy = energy;
    }

    override public string ToString()
    {
        return $"playerId:{playerId},coinVal:{coinVal},energy:{energy}";
    }


}