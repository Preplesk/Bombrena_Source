using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RandomBoxScript : NetworkBehaviour
{
    public float BoxSpawnTimer;
    public float timer;

    private void Start()
    {
        if (isServer)
        {
            timer = Time.time + BoxSpawnTimer;
        }
    }
    private void Update()
    {   if (isServer)
        {
            if (timer < Time.time)
            {                
                UnitManager.Instance.ResetUnit(gameObject.transform.position);
                //GManager.Instance.RemoveObject(gameObject);
                Destroy(gameObject);
                BoxManagement.Instance.SpawnBox(gameObject.transform.position);
            }
        }
    }


}
