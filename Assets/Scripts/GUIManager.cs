using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {

    public GameObject playerLive;
    private List<GameObject> Player1Lives = new List<GameObject>();
    private List<GameObject> Player2Lives = new List<GameObject>();

    private void Start()
    {
        for (int i = 1; i < 2; i++)
        {
            Player1Lives.Add(playerLive);
            Player2Lives.Add(playerLive);
        }
    }

    private void Update()
    {
        
    }
}
