using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Explosions : NetworkBehaviour {

    private float bombDistance = 2;    
    private float targetX; 
    private float targetY;
    public bool spreading = true;
    private float moveBy = 2.0f;
    public bool isX;  
    public bool isNegative;        
    public GameObject explosionTail;
    public GameObject bombObject;
    private float explosionDuration;
    private int counter = 0;

	
	void Start () {
        if (isServer)
        {
            explosionDuration = bombObject.GetComponent<BombBehave>().explosionDuration;
            if (isNegative)
            {
                targetX = GetComponent<Transform>().position.x - 6 * bombDistance;
                targetY = GetComponent<Transform>().position.y - 6 * bombDistance;
            }
            else
            {
                targetX = GetComponent<Transform>().position.x + 6 * bombDistance;
                targetY = GetComponent<Transform>().position.y + 6 * bombDistance;
            }
        }
    }
        
    void Update () {
        if (isServer)
        {
            if (isX)
            {
                if (spreading && targetX != transform.position.x && !isNegative)
                {
                    transform.position = new Vector2(transform.position.x + moveBy, transform.position.y);
                    RpcUpdateExplosion(transform.position);

                    counter++;
                    if (counter == 6 / moveBy)
                    {
                        var ExplosionTail = Instantiate(explosionTail, new Vector2(transform.position.x - 6, transform.position.y), Quaternion.identity);
                        NetworkServer.Spawn(ExplosionTail);
                        Destroy(ExplosionTail, explosionDuration);

                        counter = 0;
                    }
                }
                else if (spreading && targetX != transform.position.x && isNegative)
                {
                    transform.position = new Vector2(transform.position.x - moveBy, transform.position.y);
                    RpcUpdateExplosion(transform.position);

                    counter++;
                    if (counter == 6 / moveBy)
                    {
                        var ExplosionTail = Instantiate(explosionTail, new Vector2(transform.position.x + 6, transform.position.y), Quaternion.identity);
                        NetworkServer.Spawn(ExplosionTail);
                        Destroy(ExplosionTail, explosionDuration);
                        counter = 0;
                    }
                }
            }
            else
            {
                if (spreading && targetY != transform.position.y && !isNegative)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + moveBy);
                    RpcUpdateExplosion(transform.position);

                    counter++;
                    if (counter == 6 / moveBy)
                    {
                        var ExplosionTail = Instantiate(explosionTail, new Vector2(transform.position.x, transform.position.y - 6), Quaternion.identity);
                        RpcUpdateExplosion(transform.position);

                        NetworkServer.Spawn(ExplosionTail);
                        Destroy(ExplosionTail, explosionDuration);
                        counter = 0;
                    }
                }
                else if (spreading && targetY != transform.position.y && isNegative)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - moveBy);
                    RpcUpdateExplosion(transform.position);

                    counter++;
                    if (counter == 6 / moveBy)
                    {
                        var ExplosionTail = Instantiate(explosionTail, new Vector2(transform.position.x, transform.position.y + 6), Quaternion.identity);
                        NetworkServer.Spawn(ExplosionTail);
                        Destroy(ExplosionTail, explosionDuration);
                        counter = 0;
                    }
                }
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
        {
            if (collision.tag == "Obstacle" && spreading)
            {
                if (isX)
                {
                    if (!isNegative)
                    {
                        transform.position = new Vector2(transform.position.x - moveBy, transform.position.y);
                        RpcUpdateExplosion(transform.position);

                    }
                    else if (isNegative)
                    {
                        transform.position = new Vector2(transform.position.x + moveBy, transform.position.y);
                        RpcUpdateExplosion(transform.position);
                    }
                }
                else
                {
                    if (!isNegative)
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y - moveBy);
                        RpcUpdateExplosion(transform.position);
                    }
                    else if (isNegative)
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y + moveBy);
                        RpcUpdateExplosion(transform.position);
                    }
                }

                spreading = false;
            }
            if (collision.tag == "Box" && spreading)
            {
                if (isServer)
                {
                    if (isX)
                    {
                        if (!isNegative)
                        {
                            transform.position = new Vector2(transform.position.x + 5 - moveBy, transform.position.y);
                            RpcUpdateExplosion(transform.position);
                            if (bombDistance > 1)
                            {
                                var ExplosionTail = Instantiate(explosionTail, new Vector2(transform.position.x - 6, transform.position.y), Quaternion.identity);
                                NetworkServer.Spawn(ExplosionTail);
                                Destroy(ExplosionTail, explosionDuration);
                            }
                        }
                        else if (isNegative)
                        {
                            transform.position = new Vector2(transform.position.x - 5 + moveBy, transform.position.y);
                            RpcUpdateExplosion(transform.position);
                            if (bombDistance > 1)
                            {
                                var ExplosionTail = Instantiate(explosionTail, new Vector2(transform.position.x + 6, transform.position.y), Quaternion.identity);
                                NetworkServer.Spawn(ExplosionTail);
                                Destroy(ExplosionTail, explosionDuration);
                            }
                            
                        }
                    }
                    else
                    {
                        if (!isNegative)
                        {
                            transform.position = new Vector2(transform.position.x, transform.position.y + 5 - moveBy );
                            RpcUpdateExplosion(transform.position);
                            if (bombDistance > 1)
                            {
                                var ExplosionTail = Instantiate(explosionTail, new Vector2(transform.position.x, transform.position.y - 6), Quaternion.identity);
                                NetworkServer.Spawn(ExplosionTail);
                                Destroy(ExplosionTail, explosionDuration);
                            }                            
                        }
                        else if (isNegative)
                        {
                            transform.position = new Vector2(transform.position.x, transform.position.y - 5 + moveBy );
                            RpcUpdateExplosion(transform.position);
                            if (bombDistance > 1)
                            {
                                var ExplosionTail = Instantiate(explosionTail, new Vector2(transform.position.x, transform.position.y + 6), Quaternion.identity);
                                NetworkServer.Spawn(ExplosionTail);
                                Destroy(ExplosionTail, explosionDuration);
                            }
                        }
                    }
                    spreading = false;
                }
            }
        }
        
    }
    [ClientRpc]
    private void RpcUpdateExplosion(Vector2 _position)
    {
        transform.position = _position;
    }
}
