using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spawn : MonoBehaviour {

    public Vector2 spawnPosition { get; private set; }

    private void Awake()
    {
        spawnPosition = transform.position;
    }

    
}
