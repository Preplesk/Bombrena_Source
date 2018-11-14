using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTriggers : MonoBehaviour {
    public BombBehave bombBehave;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Explosion")
        {
            bombBehave.detonate = true;
        }
    }
}
