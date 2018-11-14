using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class BoxBehave : NetworkBehaviour {
    enum Items
    {
        empty,
        speed,
        boomb     
    }

    public GameObject bombItem;
    public GameObject speedItem;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
        {                  
            if (collision.tag == "Explosion")
            {
                GManager.Instance.RemoveObject(gameObject);
                //GManager.RemoveObject(gameObject);
                int generated = Random.Range(0, 3);

                switch (generated)
                {
                    case 0:
                        break;
                    case 1:
                        var speed = Instantiate(speedItem,transform.position,Quaternion.identity);
                        GManager.Instance.AddField(speed);
                        //GManager.fields.Add(new Field(speed.transform.position, speed));
                        NetworkServer.Spawn(speed);
                        break;
                    case 2:
                        var bomb = Instantiate(bombItem,transform.position,Quaternion.identity);
                        GManager.Instance.AddField(bomb);
                        //GManager.fields.Add(new Field(bomb.transform.position, bomb));
                        NetworkServer.Spawn(bomb);
                        break;

                    default: break;
                }
            }
        }           
    }    
}
