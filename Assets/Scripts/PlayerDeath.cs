using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDeath : NetworkBehaviour {

    public Rigidbody Head;
    public Rigidbody RightHeand;
    public Rigidbody LeftHeand;
    public Rigidbody Body;
    public Rigidbody RightLeg;
    public Rigidbody LeftLeg;
    public PlayerControler player;
    public float ExplodePower;
    public float RotationPower;
    public GameObject deadPlayer;
    public GameObject WinBoard;
    public GameObject LoseBoard;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
        {
            if (collision.tag == "Explosion" && player.alive)
            {
                player.alive = false;
                CmdKillPlayer();
            }
        }
    }

    [ClientRpc]
    private void RpcUpdatePlayerDeath()
    {
        player.alive = false;
        player.playerSprite.enabled = false;
        deadPlayer.SetActive(true);
        AudioManager.Instance.CmdPlay("scream");
        player.playerLives -= 1;
        ExplodeCharacter();
    }

    [Command]
    private void CmdKillPlayer()
    {
        RpcUpdatePlayerDeath();       
        if (player.playerLives == 1)
        {
            GManager.Instance.ChangeGameStatus( GManager.GameStatus.endGame);
        }
        else
        {
           GManager.Instance.ChangeGameStatus(GManager.GameStatus.endTurn);
        }
        GManager.Instance.resetTimer = Time.time + GManager.Instance.resetDelay;
    }

    private void ExplodeCharacter()
    {
        ExplodePower *= Time.deltaTime;
        RotationPower *= Time.deltaTime;
        Head.GetComponent<SpriteRenderer>().color = player.playerColor;
        Head.AddForce(new Vector3(Random.Range(-10,10) * ExplodePower, Random.Range(0,30) * ExplodePower, Random.Range(-1,-5) *ExplodePower),ForceMode.Force);
        Head.AddTorque(transform.up * (float)Random.Range(-10, 10) * RotationPower);
        Head.AddTorque(transform.right * (float)Random.Range(-10, 10) * RotationPower);
        Body.GetComponent<SpriteRenderer>().color = player.playerColor;
        Body.AddForce(new Vector3(Random.Range(-10, 10) * ExplodePower, Random.Range(-20, 20 ) * ExplodePower, Random.Range(-1, -5) * ExplodePower), ForceMode.Force);
        Body.AddTorque(transform.up * (float)Random.Range(-10, 10) * RotationPower);
        Body.AddTorque(transform.right * (float)Random.Range(-10, 10) * RotationPower);
        LeftHeand.GetComponent<SpriteRenderer>().color = player.playerColor;
        LeftHeand.AddForce(new Vector3(Random.Range(-60, -1) * ExplodePower, Random.Range(-20, 20) * ExplodePower, Random.Range(-1, -5) * ExplodePower), ForceMode.Force);
        LeftHeand.AddTorque(transform.up * (float)Random.Range(-10, 10) * RotationPower);
        LeftHeand.AddTorque(transform.right * (float)Random.Range(-10, 10) * RotationPower);
        RightHeand.GetComponent<SpriteRenderer>().color = player.playerColor;
        RightHeand.AddForce(new Vector3(Random.Range(1, 60) * ExplodePower, Random.Range(-20, 20) * ExplodePower, Random.Range(-1, -5) * ExplodePower), ForceMode.Force);
        RightHeand.AddTorque(transform.up * (float)Random.Range(-10, 10) * RotationPower);
        RightHeand.AddTorque(transform.right * (float)Random.Range(-10, 10) * RotationPower);
        RightLeg.GetComponent<SpriteRenderer>().color = player.playerColor;
        RightLeg.AddForce(new Vector3(Random.Range(1, 60) * ExplodePower, Random.Range(-1, -20) * ExplodePower, Random.Range(-1, -5) * ExplodePower), ForceMode.Force);
        RightLeg.AddTorque(transform.up * (float)Random.Range(-10, 10) * RotationPower);
        RightLeg.AddTorque(transform.right * (float)Random.Range(-10, 10) * RotationPower);
        LeftLeg.GetComponent<SpriteRenderer>().color = player.playerColor;
        LeftLeg.AddForce(new Vector3(Random.Range(-60, -1) * ExplodePower, Random.Range(-1, -20) * ExplodePower, Random.Range(-1, -5) * ExplodePower),ForceMode.Force);
        LeftLeg.AddTorque(transform.up * (float)Random.Range(-10, 10) * RotationPower);
        LeftLeg.AddTorque(transform.right * (float)Random.Range(-10, 10) * RotationPower);
    }
    public void CharacterPartsReset()
    {       
        Head.transform.position = gameObject.transform.position;      
        Body.transform.position = gameObject.transform.position;       
        LeftHeand.transform.position = gameObject.transform.position;      
        RightHeand.transform.position = gameObject.transform.position;       
        LeftLeg.transform.position = gameObject.transform.position;       
        RightLeg.transform.position = gameObject.transform.position;
    }
}
