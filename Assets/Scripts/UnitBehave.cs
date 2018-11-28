﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnitBehave : NetworkBehaviour {    

    private bool unitActive = false;    
    public GameObject plant = null;
    public GameObject contentObject = null;
    public UnitManager.UnitContent content = UnitManager.UnitContent.empty;
    public GameObject player = null;
    public GameObject bomb;
 
    private void Update()
    {
        if (unitActive)
        {
            GameObject player = plant.transform.root.gameObject;
            Debug.Log("Checking");
            if (player.GetComponent<PlayerControler>().plant)
            {
                Debug.Log("Planting");                
                player.GetComponent<PlayerControler>().CmdAddBomb(transform.position, player);
            }
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {     

        if (collision.name == "PlantPlace")
        {
            Debug.Log("Unit set active on :" + transform.position);
            unitActive = true;
            plant = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "PlantPlace")
        {
            Debug.Log("Unit deactivate  :" + transform.position);
            unitActive = false;
            plant = null;
        }
    }

    public void Empty()
    {
        if (player != null)
        {
            player.GetComponent<PlayerControler>().bombLimit += 1;
        }
        Destroy(contentObject);
        contentObject = null;
        content = UnitManager.UnitContent.empty;
        player = null;    
    }    
}
