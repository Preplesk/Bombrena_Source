using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : NetworkBehaviour {

    public static UIManager Instance { get; private set; }
    public Text player0Name;
    public Text player1Name;
    public Image player0Live1;
    public Image player0Live2;
    public Image player1Live1;
    public Image player1Live2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ClientRpc]
    public void RpcSetPlayerLive(int lives, int playerId)
    {
        if (playerId == 0)
        {
            if (lives == 2)
            {
                player0Live1.enabled = false;
            }
            else if (lives == 1)
            {
                player0Live2.enabled = false;
            }
        }
        else if (playerId == 1)
        {
            if (lives == 2)
            {
                player1Live2.enabled = false;
            }
            else if (lives == 1)
            {
                player1Live1.enabled = false;
            }
        }
        else
        {
            Debug.Log("Wrong player id");
        }
    }   

    
    public void SetPlayerName(string _name, int playerId, Color32 color)
    {
        if (playerId == 0)
        {
            player0Name.text = _name;
            player0Name.color = color;
            player0Live1.color = color;
            player0Live2.color = color;
        }
        else if (playerId == 1)
        {
            player1Name.text = _name;
            player1Name.color = color;
            player1Live1.color = color;
            player1Live2.color = color;
        }
        else
        {
            Debug.Log("Wrong player id");
        }
    }
}
