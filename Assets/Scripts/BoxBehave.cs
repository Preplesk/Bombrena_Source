using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class BoxBehave : NetworkBehaviour {
    
    public GameObject bombItem;
    public GameObject speedItem;
    public GameObject powerItem;

    public void SpawnItem(GameObject item)
    {
        var newItem = Instantiate(item, transform.position, Quaternion.identity);
        UnitManager.Instance.ChangeUnitState(gameObject.transform.position, newItem, UnitManager.UnitContent.item, null);
        NetworkServer.Spawn(newItem);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
        {                  
            if (collision.tag == "Explosion")
            {
                UnitManager.Instance.ResetUnit(gameObject.transform.position);
                int generated = Random.Range(0, 5);

                switch (generated)
                {
                    case 0:
                        break;
                    case 1:
                        SpawnItem(speedItem);
                        break;
                    case 2:
                        SpawnItem(bombItem);
                        break;
                    case 3:
                        SpawnItem(powerItem);
                        break;

                    default: break;
                }
            }            
        }           
    }    
}
