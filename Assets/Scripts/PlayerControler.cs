using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControler : NetworkBehaviour {

    Rigidbody2D Player;
    Vector2 input;
    Transform Collider;
    Animator playerAnimator;

    // Player movement variables
    public SpriteRenderer playerSprite;
    float horizontal;
    float vertical;
    public float tolerance;
    [SyncVar]
    private bool isRight = true;
    [SyncVar]
    public bool alive = true;
    bool isColliding;

    // Lives     
    [SyncVar]
    public int playerLives;    

    // Player bomb management 
    public GameObject bomb;
    [SyncVar]
    public bool plant;
    

    //Player properties
    private Vector2 defaultPosition;
    public int Id;
    public float defaultSpeed ;
    [HideInInspector]
    public float speed;
    public int defaultBombLimit;
    [HideInInspector]
    [SyncVar]
    public int bombLimit;
    [HideInInspector]
    [SyncVar]
    public float bombPower;
    public float defaultBombPower; // and this
    [HideInInspector]
    [SyncVar]
    public bool canKick = false;

    // Player settings 
    [SyncVar] 
    public GameObject Settings;
    [SyncVar]
    public string playerName;
    [SyncVar]
    public Color32 playerColor;
    [SyncVar]
    public bool playerReady = false;

    void Start()
    {
        defaultPosition = transform.position;
        Player = gameObject.GetComponent<Rigidbody2D>();
        playerAnimator = gameObject.GetComponent<Animator>();
        GManager.Instance.AddPlayer(gameObject);
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        speed = defaultSpeed;
        bombPower = defaultBombPower;
        bombLimit = defaultBombLimit;
        playerLives = GManager.Instance.playersLives;
        isColliding = false;

        if (isLocalPlayer)
        {
            Settings.SetActive(true);
        }
    }

    private void Update()
    {
        if (alive && GManager.Instance.GetGameState() == GManager.GameStatus.start)
        {
            if (!isLocalPlayer)
            {
                return;
            }


            if (Input.GetButtonDown("PlaceBomb") && bombLimit > 0)
            {
                plant = true;
            }
            else plant = false;
        }
    }

    void FixedUpdate()
    {
        if (alive && GManager.Instance.GetGameState() == GManager.GameStatus.start)
        {
            if (!isLocalPlayer)
            {
                return;
            }

            horizontal = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
            vertical = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

            if (horizontal != 0 || vertical != 0)
            {
                if (vertical > 0)
                {
                    playerAnimator.SetBool("RunYUp", true);
                    playerAnimator.SetBool("RunYDown", false);
                    playerAnimator.SetBool("RunX", false);

                }
                else if (vertical < 0)
                {
                    playerAnimator.SetBool("RunYUp", false);
                    playerAnimator.SetBool("RunYDown", true);
                    playerAnimator.SetBool("RunX", false);
                }
                else
                {
                    playerAnimator.SetBool("RunX", true);
                    playerAnimator.SetBool("RunYDown", false);
                    playerAnimator.SetBool("RunYUp", false);

                    if (horizontal > 0 && !isRight)
                    {
                        CmdTurn();
                        isRight = true;
                    }
                    else if (horizontal < 0 && isRight)
                    {
                        CmdTurn();
                        isRight = false;
                    }
                }
            }

            else
            {
                playerAnimator.SetBool("RunX", false);
                playerAnimator.SetBool("RunYUp", false);
                playerAnimator.SetBool("RunYDown", false);
            }


            Vector2 moveBy = new Vector2(horizontal, vertical);

            // Player movement assistant 
            if (isColliding && horizontal != 0)
            {
                if (Collider.position.y + tolerance < transform.position.y
                    && transform.position.y < Collider.position.y + 6.0f) //obstacle collider size y + player colider size y /2  <- Rework that later...
                {
                    moveBy = new Vector2(horizontal, Mathf.Abs(horizontal));
                }
                else if (Collider.position.y - tolerance > transform.position.y
                    && transform.position.y > Collider.position.y - 5f) // - offset
                {
                    moveBy = new Vector2(horizontal, -Mathf.Abs(horizontal));
                }
            }
            else if (isColliding && vertical != 0)
            {
                if (Collider.position.x + tolerance < transform.position.x
                    && transform.position.x < Collider.position.x + 4.75f)
                {
                    moveBy = new Vector2(Mathf.Abs(vertical), vertical);
                }
                else if (Collider.position.x - tolerance > transform.position.x
                    && transform.position.x > Collider.position.x - 4.45f)
                {
                    moveBy = new Vector2(-Mathf.Abs(vertical), vertical);
                }
            }
            Player.velocity = moveBy;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Obstacle")
        {
            isColliding = true;
            Collider = collision.collider.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }

    [ClientRpc]
    public void RpcUpdatePlayerScreen()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (alive)
        {
            gameObject.GetComponent<PlayerDeath>().WinBoard.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<PlayerDeath>().LoseBoard.SetActive(true);
        }
    }
    [ClientRpc]
    public void RpcResetPlayerScreen()
    {
        gameObject.GetComponent<PlayerDeath>().WinBoard.SetActive(false);
        gameObject.GetComponent<PlayerDeath>().LoseBoard.SetActive(false);
    }

    [ClientRpc]
    public void RpcResetPlayer()
    {
        gameObject.GetComponent<PlayerDeath>().LoseBoard.SetActive(false);
        gameObject.GetComponent<PlayerDeath>().WinBoard.SetActive(false);
        playerSprite.enabled = true;
        gameObject.GetComponent<PlayerDeath>().CharacterPartsReset();
        gameObject.GetComponent<PlayerDeath>().deadPlayer.SetActive(false);
        transform.position = defaultPosition;
        alive = true;
        bombLimit = defaultBombLimit;
        bombPower = defaultBombPower;
        canKick = false;
        speed = defaultSpeed;
    }
    [ClientRpc]
    public void RpcResetPlayerLives()
    {
        playerLives = GManager.Instance.playersLives;
    }

    [Command]
    public void CmdAddBomb(Vector2 _plant, GameObject _player)
    {
        bool isFree = UnitManager.Instance.IsEmpty(_plant); 

        if (isFree)
        {

            var _bomb = Instantiate(bomb, _plant, Quaternion.identity);
            _bomb.GetComponent<BombBehave>().explosionPower = bombPower;
            UnitManager.Instance.ChangeUnitState(_plant, _bomb, UnitManager.UnitContent.bomb, _player);
            NetworkServer.Spawn(_bomb);
            AudioManager.Instance.RpcPlay("cackle");
            _player.GetComponent<PlayerControler>().bombLimit -= 1;
        }
    }

    [Command]
    public void CmdPlayerSettings( Color32 color, string name )
    {
        RpcPlayerSettings(color, name);
    }
        
    [ClientRpc]
    public void RpcPlayerSettings( Color32 color, string name )
    {
        
        playerReady = true;
        playerName = name;
        playerColor = color;
        playerSprite.color = color;
        UIManager.Instance.SetPlayerName(playerName, Id, playerColor);
        
    }

    [Command]
    void CmdTurn()
    {
        RpcTurn();
    }

    [ClientRpc]
    void RpcTurn()
    {
        var temp = transform.localScale;
        temp.x *= -1;
        transform.localScale = temp;
    }
}