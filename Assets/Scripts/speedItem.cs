using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedItem : MonoBehaviour {

    public float speedIncrease;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerControler>().speed += speedIncrease;
            GManager.Instance.RemoveObject(gameObject);
        }
    }
}
