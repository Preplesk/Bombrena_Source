using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Obstacle: MonoBehaviour
{        
    public Vector2 ObstaclePos { get; private set; }

    private void Awake()
    {
        ObstaclePos = gameObject.transform.position;
    }
}
