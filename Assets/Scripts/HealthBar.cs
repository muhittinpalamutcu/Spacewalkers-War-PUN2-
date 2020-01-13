using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HealthBar : MonoBehaviourPun
{

    public Image fillImage;
    public float health = 1;
    public BoxCollider2D boxCollider;
    public Rigidbody2D rigid;
    public SpriteRenderer sprite;
    public GameObject playerCanvas;

    public SpaceMan playerScript;
   
    float sum = 0f;

    
    //deneme tekrar
    public GameObject KillGotKilledText;

    private float Timex = 5f;
    public bool StartLoadScene;





    void Update()
    {
        if (StartLoadScene)
        {
            startReturningLobby();

        }
    }

    [PunRPC]
    public void EnableLobby(bool check)
    {
        Timex = 5;
        StartLoadScene = check;
    }

    public void startReturningLobby()
    {
        Timex -= Time.deltaTime;

        if (Timex <= 0)
        {

            StartLoadScene = false;

            this.GetComponent<PhotonView>().RPC("Endgame", RpcTarget.AllBuffered);


        }
    }


    public void CheckHealth()
    {
        if (photonView.IsMine && health <= 0)
        {

            GameManager.reference.EnableRespawn(); //we need to only enable screen on our local player 
            playerScript.DisableInput = true;
            this.GetComponent<PhotonView>().RPC("death", RpcTarget.AllBuffered);

        }



    }

    [PunRPC]
    public void death()
    {
        rigid.gravityScale = 0;
        boxCollider.enabled = false;
        sprite.enabled = false;
        playerCanvas.SetActive(false); //when the player death disable to character


    }

    [PunRPC]
    public void Revive()
    {

        rigid.gravityScale = 0;
        boxCollider.enabled = true;
        sprite.enabled = true;
        playerCanvas.SetActive(true);
        fillImage.fillAmount = 1;
        health = 1;

    }

    public void EnableInputs()
    {
        playerScript.DisableInput = false;
    }

    [PunRPC]
    public void HealthUpdate(float damage)
    {
        fillImage.fillAmount -= damage;
        health = fillImage.fillAmount;
        if (health == 0)
        {
            rigid.gravityScale = 0;
            boxCollider.enabled = false;
            sprite.enabled = false;
            playerCanvas.SetActive(false);
        }
        CheckHealth();
    }


  

    [PunRPC]
    void Endgame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);   
    }

    
    [PunRPC]
    void callWin(string name)
    {
       GameManager.reference.WinScreen(name);
    }
  

  

    [PunRPC]
    void KillCounter(string name)
    {
        float x = 1f;
        sum = sum + x;
        
        

        GameObject go = Instantiate(KillGotKilledText, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(GameManager.reference.KillGotKilledFeedBox.transform, false);
       
        go.GetComponent<Text>().text = "Kill: " +sum;
        Destroy(go, 3);
        

        if (sum == 10)
        {
            this.GetComponent<PhotonView>().RPC("EnableLobby", RpcTarget.AllBuffered, true);
            this.GetComponent<PhotonView>().RPC("callWin",RpcTarget.AllBuffered, name);
   
        }
    }

    [PunRPC]
    void deathCounter()
    {
       
    }




}
