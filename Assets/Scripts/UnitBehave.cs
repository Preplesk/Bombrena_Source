using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnitBehave : NetworkBehaviour {

    private bool unitActive = false;    
    private GameObject plant = null;
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
    
}
