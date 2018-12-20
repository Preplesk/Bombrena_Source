using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class GManager : NetworkBehaviour {

    public enum GameStatus
    {
        start,
        notStart,
        endTurn,
        endGame
    }

    public static GManager Instance { get; private set; }

    public List<GameObject> players = new List<GameObject>();
    public GameStatus gameStatus = GameStatus.notStart;
    public GameObject WaitForOthers; 
    public GameObject LookingForOthers;
    public float gameResetTimer; 
    public int playersLives;
    public int maxPlayers;
    public float turnResetTimer;
    [HideInInspector]
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
            gameStatus = GameStatus.start;
            CmdResetGame();
        }
    }

    /*-------------------------------------------------------------------------*/
    /*-------------------------------------------------------------------------*/

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

   
    public void ChangeGameStatus( GManager.GameStatus newStatus )
    {
        gameStatus = newStatus;
    }

    public GameStatus GetGameState()
    {
        return gameStatus;
    }

    
    [Command]
    private void CmdResetGame()
    {
        UnitManager.Instance.ResetUnits();
        BoxManagement.Instance.SpawnBoxes();
        for (int i = 0; i < players.Count; i++)
        {
            var controller = players[i].GetComponent<PlayerControler>();
            controller.RpcResetPlayer();
            controller.RpcResetPlayerLives();           
            controller.RpcResetPlayerScreen(); 
        }
        UIManager.Instance.RpcResetLives();
    }
}
