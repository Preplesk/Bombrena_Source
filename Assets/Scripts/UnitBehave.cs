using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehave : MonoBehaviour {
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlantPlace")
        {
            var player = collision.transform.parent.gameObject;
            player.GetComponent<PlayerControler>().plant = new Vector2(transform.position.x, transform.position.y);
            Debug.Log("Plant set to :" + transform.position);
        }
    }
}
