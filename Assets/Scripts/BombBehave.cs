using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BombBehave : NetworkBehaviour
{

    public bool detonate = false;
    public bool explosion = false;
    public GameObject Bomb;
    public float explosionTimer = 3;
    public float explosionDuration = 1;
    private float explosionTime;
    public GameObject ExplosionRight;
    public GameObject ExplosionLeft;
    public GameObject ExplosionUp;
    public GameObject ExplosionDown;

    void Start()
    {
        if (isServer)
        {
            explosionTime = Time.time + explosionTimer;
        }
    }


    void Update()
    {
        if (isServer)
        {
            if (explosionTime < Time.time) detonate = true;
            if (detonate && !explosion)
            {
                RpcUpdateClientBomb();
                AudioManager.Instance.RpcPlay("explosion");
                SpawnExplosions();
                
            }
            if (explosion && explosionTime < Time.time)
            {                
                GManager.Instance.RemoveObject(gameObject);
            }
        }
    }
    private void SpawnExplosions()
    {
        var Explosion = Instantiate(ExplosionRight, transform.position, Quaternion.identity);
        NetworkServer.Spawn(Explosion);
        Destroy(Explosion,explosionDuration);

        Explosion = Instantiate(ExplosionLeft, transform.position, Quaternion.identity);
        NetworkServer.Spawn(Explosion);
        Destroy(Explosion, explosionDuration);

        Explosion = Instantiate(ExplosionUp, transform.position, Quaternion.identity);
        NetworkServer.Spawn(Explosion);
        Destroy(Explosion, explosionDuration);

        Explosion = Instantiate(ExplosionDown, transform.position, Quaternion.identity);
        NetworkServer.Spawn(Explosion);
        Destroy(Explosion, explosionDuration);
    }

    [ClientRpc]
    private void RpcUpdateClientBomb()
    {
        Destroy(Bomb);
        explosionTime = Time.time + explosionDuration;
        explosion = true;
    }
}
