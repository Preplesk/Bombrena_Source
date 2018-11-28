using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoxManagement : NetworkBehaviour {

    public static BoxManagement Instance { get; private set; }

    public int width;
    public int height;
    public bool maxPlayers = false;
    public float boxSpawnTimer;
    [SerializeField]
    private float timer;
    public GameObject Box;
    public GameObject newBox;
    public Transform spawn1 ;
    public Transform spawn2 ;
    public Transform spawn3 ;
    public Transform spawn4 ;
    public int spawnRange;
    public Obstacle[] obstecles;
    List<Transform> spawns = new List<Transform>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
    private void Start()
    {
        if (isServer)
        {
            if (GManager.Instance.gameStatus == GManager.GameStatus.start)
            {                
                timer = Time.time + boxSpawnTimer;
            }
        }
    }

    private void Update()
    {   
        
        if (isServer)
        {
            if (GManager.Instance.gameStatus == GManager.GameStatus.start)
            {
                if (timer < Time.time)
                {
                    Debug.Log("Call for spawn new box");
                    SpawnRandomBox();
                    timer = Time.time + boxSpawnTimer;
                }
            }
        }
    
    }

    private bool SpawnRate()
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
               
                if (SpawnRate() && !CheckSpawn(tempPosition))
                {
                    SpawnBox(tempPosition);
                }
            }
        }
    }

    public void SpawnRandomBox()
    {
        Vector2 spawn;
        
        float y = Random.Range(height / -2, height / 2);
        float x = Random.Range(width / -2, width / 2);
        spawn = new Vector2(x*6, y*6);
                
        if ((UnitManager.Instance.IsEmpty(spawn) && !CheckSpawn(spawn) && CheckObstacles(spawn)))
        {
            SpawnBox(spawn, newBox);
        }
    }

    public void SpawnBox(Vector2 pos)
    {
        var box = Instantiate(Box, pos, Quaternion.identity);
        UnitManager.Instance.ChangeUnitState(pos, box, UnitManager.UnitContent.box, null);
        //GManager.Instance.AddField(box);
        NetworkServer.Spawn(box);
    }

    public void SpawnBox(Vector2 pos, GameObject newBox)
    {
        Debug.Log("Inside spawning fcion");
        var box = Instantiate(newBox, pos, Quaternion.identity);
        UnitManager.Instance.ChangeUnitState(pos, box, UnitManager.UnitContent.box, null);
        //GManager.Instance.AddField(box);
        NetworkServer.Spawn(box);
    }

    private bool CheckObstacles(Vector2 pos)
    {
        foreach (Obstacle obs in obstecles)
        {
            if (obs.ObstaclePos == pos) return false;
        }

        return true;        
    }
}
