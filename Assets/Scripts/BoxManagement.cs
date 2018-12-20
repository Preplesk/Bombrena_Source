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
    private float timer;
    public GameObject Box;
    public GameObject newBox;
    public int spawnRange;
    public Obstacle[] obstecles;
    public Spawn[] spawns;

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
        for (int i = 0; i < GManager.Instance.maxPlayers; i++)
        {
            if (spawns[i].spawnPosition.x - 6 <= position.x &&
                spawns[i].spawnPosition.x + 6 >= position.x &&
                spawns[i].spawnPosition.y - 6 <= position.y &&
                spawns[i].spawnPosition.y + 6 >= position.y)
                return true;
        }
        return false;
    }

    public void SpawnBoxes()
    {
        for (int y = height / -2; y <= height / 2; y++)
        {
            for (int x = width / 2; x >= width / -2; x--)
            {
                Vector2 spawn = new Vector2(x * 6, y * 6);
               
                if (SpawnRate() && !CheckSpawn(spawn) && CheckObstacles(spawn))
                {
                    SpawnBox(spawn, Box);
                }
            }
        }
    }

    public void SpawnRandomBox()
    {
        Vector2 spawn;        
        
        spawn = UnitManager.Instance.GetRandomUnitPos();
                
        if ((UnitManager.Instance.IsEmpty(spawn) && !CheckSpawn(spawn) && CheckObstacles(spawn)))
        {
            SpawnBox(spawn, newBox);
        }
    }

    public void SpawnBox(Vector2 pos, GameObject _box)
    {
        if (_box == null) _box = Box;
        var box = Instantiate(_box, pos, Quaternion.identity);
        UnitManager.Instance.ChangeUnitState(pos, box, UnitManager.UnitContent.box, null);
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
