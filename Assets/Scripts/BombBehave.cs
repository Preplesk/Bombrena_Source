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
    public float explosionPower; 
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
                SpawnExplosions(explosionPower,transform.position);
                
            }
            if (explosion && explosionTime < Time.time)
            {
                UnitManager.Instance.ResetUnit(gameObject.transform.position);
                //GManager.Instance.RemoveObject(gameObject);
            }
        }
    }
    public void SpawnExplosions(float explosionPower, Vector2 pos)
    {
        var Explosion = Instantiate(ExplosionRight, pos, Quaternion.identity);
        Explosion.GetComponent<Explosions>().explosionDistance = explosionPower;
        NetworkServer.Spawn(Explosion);
        Destroy(Explosion,explosionDuration);

        Explosion = Instantiate(ExplosionLeft, pos, Quaternion.identity);
        Explosion.GetComponent<Explosions>().explosionDistance = explosionPower;
        NetworkServer.Spawn(Explosion);
        Destroy(Explosion, explosionDuration);
                
        Explosion = Instantiate(ExplosionUp, pos, Quaternion.identity);
        Explosion.GetComponent<Explosions>().explosionDistance = explosionPower;
        NetworkServer.Spawn(Explosion);
        Destroy(Explosion, explosionDuration);
                
        Explosion = Instantiate(ExplosionDown, pos, Quaternion.identity);
        Explosion.GetComponent<Explosions>().explosionDistance = explosionPower;
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
