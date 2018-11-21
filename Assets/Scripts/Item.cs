using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Item : NetworkBehaviour {

    public bool speed;
    public float speedIncrease;
    public bool bomb;
    public GameObject bombObject;
    public BombBehave bombBehave;
    public bool canBeDestroyed ;
    public float saveTime;
    public float timer;

    private void Start()
    {
        if (isServer)
        {
            timer = saveTime + Time.time;
            canBeDestroyed = false;
        }
    }

    private void Update()
    {
        if (isServer)
        {
            if (timer < Time.time)
            {
                canBeDestroyed = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
        {

            if (collision.tag == "Player" && speed)
            {
                collision.GetComponent<PlayerControler>().speed += speedIncrease;
                GManager.Instance.RemoveObject(gameObject);
            }
            else if (collision.tag == "Player" && bomb)
            {
                collision.GetComponent<PlayerControler>().bombLimit += 1;
                GManager.Instance.RemoveObject(gameObject);
            }
            else if (collision.tag == "Explosion" && canBeDestroyed)
            {
                if (bomb)
                {
                    bombObject.GetComponent<BombBehave>().SpawnExplosions(10.0f, transform.position);
                }
                GManager.Instance.RemoveObject(gameObject);
            }
        }
    }
}
