using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{

    public PlayerControler player;   
    public Color32 playerColor;
    public string playerName;
    public Text text;

    
    public void SetPlayerSettings()
    {           
        player.playerReady = true;
        gameObject.SetActive(false);
        player.CmdPlayerSettings(playerColor, playerName);        
    }

    public void SetName()
    {
        playerName = text.text;
    }
}
