using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoxManagement : NetworkBehaviour {

    public int width;
    public int height;
    public bool maxPlayers = false;
    public GameObject Box;
    public Transform spawn1 ;
    public Transform spawn2 ;
    public Transform spawn3 ;
    public Transform spawn4 ;
    public int spawnRange;
    List<Transform> spawns = new List<Transform>();

    // 
    public override void OnStartServer()
    {
        
        if (maxPlayers)
        {
            spawns.Add(spawn1);
            spawns.Add(spawn2);
            spawns.Add(spawn3);
            spawns.Add(spawn4);
        }
        else
        {
            spawns.Add(spawn1);
            spawns.Add(spawn2);
        }
        SpawnBoxes();        
    }
    
    private bool SpawnRandom()
    {
        int n = Random.Range(0, spawnRange);
        if (n == 0)
        {
            return true;
        }
        return false;
    } 
    

    private bool CheckSpawn(Vector2 position)
    {
        foreach (Transform pos in spawns)
        {
            if (pos.position.x - 6 <= position.x && pos.position.x + 6 >= position.x 
                && pos.position.y - 6 <= position.y && pos.position.y + 6 >= position.y   ) return true;
        }
        return false;       
    }

    public void SpawnBoxes()
    {
        for (int y = height / -2; y <= height / 2; y++)
        {
            for (int x = width / 2; x >= width / -2; x--)
            {
                Vector2 tempPosition = new Vector2(x * 6, y * 6);
               
                if (SpawnRandom() && !CheckSpawn(tempPosition))
                {
                    SpawnBox(tempPosition);
                }

            }
        }
    }

    private void SpawnBox(Vector2 pos)
    {
        var box = Instantiate(Box, pos, Quaternion.identity);
        GManager.Instance.AddField(box);
        NetworkServer.Spawn(box);
    }
}
