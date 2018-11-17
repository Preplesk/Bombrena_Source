using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

 public class Field
{
    private Vector2 position;    
    private GameObject contentObject = null;
    private GameObject player = null;

    
    public Field(GameObject _newObject)
    {
        this.position = _newObject.transform.position;
        this.contentObject = _newObject;
        
    }

    public Field(GameObject _newObject , GameObject _player) 
    {
        this.position = _newObject.transform.position;
        this.contentObject = _newObject;
        this.player = _player;
    }
    public GameObject GetContent()
    {
        return contentObject;
    }

    public bool CheckPlayer()
    {
        if (player != null) return true;
        else return false;
    }

    public bool MatchPosition(Vector2 _position)
    {
        if (_position == this.position) { return true; }
        else return false;
    }
    public void BombLimit(int i)
    {
        player.GetComponent<PlayerControler>().bombLimit += i;
    }
}


/*-----------------------------------------------------------------*/
public class GManager : NetworkBehaviour {

    public enum GameStatus
    {
        start,  
        notStart,             
        endTurn,
        endGame
    }

    public static GManager Instance { get; private set; }

    public List<Field> fields = new List<Field>();
    public List<GameObject> players = new List<GameObject>();
    public GameStatus gameStatus = GameStatus.notStart;
    public GameObject WaitForOthers;
    public GameObject LookingForOthers;
    public float gameResetTimer = 3.0f;
    public int playersLives = 3;
    public int maxPlayers = 2;
    public int pointsToWin = 3;    
    public float turnResetTimer;
    public float timer;

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

    private void Start()
    {
        LookingForOthers.SetActive(true);
        WaitForOthers.SetActive(true);
    }

    private void Update()
    {
        Debug.Log(gameStatus);
        if (players.Count == maxPlayers)
        {
            LookingForOthers.SetActive(false);
        }
                
        if (players.Count == maxPlayers && gameStatus == GameStatus.notStart)
        {
            bool gameReady = true;
           

            foreach (GameObject player in players)
            {
                if (!player.GetComponent<PlayerControler>().playerReady)
                {
                    gameReady = false;
                }                
            }

            if (gameReady)
            {
                WaitForOthers.SetActive(false);
                gameStatus = GameStatus.start;                
            } 
        }
                
        if (gameStatus == GameStatus.endTurn)
        {
            if (Time.time > timer)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    PlayerControler player = players[i].GetComponent<PlayerControler>();
                    if (!player.alive)
                    {
                        UIManager.Instance.RpcSetPlayerLive(player.playerLives, player.Id);
                        players[i].GetComponent<PlayerControler>().RpcResetPlayer();
                    }
                    
                }
                gameStatus = GameStatus.start;
            }
        }
        if (gameStatus == GameStatus.endGame)
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerControler>().RpcUpdatePlayerScreen();
            }            
        }

        if (gameStatus == GameStatus.endGame && timer < Time.time)
        {
            Debug.Log("Rdy to reset");
            gameStatus = GameStatus.start;
            CmdResetGame();
        }
    }

    public void AddPlayer(GameObject _player)
    {
        PlayerControler player = _player.GetComponent<PlayerControler>();
        if (_player.transform.position.x < 0) player.Id = 0;
        else player.Id = 1;    
        if (maxPlayers > players.Count)
        {
            players.Add(_player);
        }
    }

    public void AddField(GameObject _newField) 
    {
        fields.Add(new Field(_newField));
    }

    public void AddField(GameObject _newFieldObject, GameObject _player)
    {
        fields.Add(new Field(_newFieldObject, _player));
    }

    public bool CheckFields(Vector2 checkThis)
    {
        foreach (Field field in fields)
        {
            if (field.MatchPosition(checkThis))
            {
                return false;
            } 
        }
        return true;
    }

    public void ChangeGameStatus( GManager.GameStatus newStatus )
    {
        gameStatus = newStatus;
    }

    public GameStatus GetGameState()
    {
        return gameStatus;
    }

    public void RemoveObject( GameObject toRemove )
    {
        foreach (Field field in fields)
        {
            if (field.GetContent() == toRemove )
            {
                if (field.CheckPlayer())
                {
                    field.BombLimit(1);
                }
                
                Destroy(field.GetContent());
                fields.Remove(field);
                return;
            }
        }
    }
    
    public void RemoveAllObjects()
    {
        foreach (Field field in fields)
        {            
            Destroy(field.GetContent());
        }
        fields = new List<Field>();
    }

    [Command]
    private void CmdResetGame()
    {
        Debug.Log("Enter reseting");
        RemoveAllObjects();
        BoxManagement.Instance.SpawnBoxes();
        Debug.Log("Box reseted");
        for (int i = 0; i < players.Count; i++)
        {
            var controller = players[i].GetComponent<PlayerControler>();
            controller.RpcResetPlayer();
           // Debug.Log("Player reseted");
            controller.RpcResetPlayerLives();
           // Debug.Log("Player lives reseted");
            //controller.RpcUpdatePlayerScreen();            
            controller.RpcResetPlayerScreen();
          //  Debug.Log("Screen reseted");            
        }
        UIManager.Instance.RpcResetLives();
    }
}
