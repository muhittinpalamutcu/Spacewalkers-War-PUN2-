using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPun
{
    public bool MovingDirection;
    public float MoveSpeed = 8;
    public float bulletDamage = 0.25f;
    public float DestroyTime = 2f;
  


    public GameObject localPlayerObj;
    //for kill Counter
    public string killerName;

    
    

    void Start()
    {
        if (photonView.IsMine) { 
        killerName = localPlayerObj.GetComponent<SpaceMan>().MyName;
        }
    }




    //for destroy bullet
    IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(DestroyTime);
        this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
    }

    void Update()
    {


        if (!MovingDirection)
        {
            transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
        }




        Destroy(gameObject, 1);
      



    }

    [PunRPC] //for changing to direction of the bullet we are checkkign
    public void ChangeDirection()
    {
        MovingDirection = true;
    }

    [PunRPC]
    void Destroy()
    {
        Destroy(this.gameObject);
    }


    //We want to destroy our bullet when it hits box collider 2d

    void OnTriggerEnter2D(Collider2D collision)
    {
        /*  if (PhotonNetwork.IsMasterClient)
      {
          if (collision.gameObject.tag == "Block")
          {
              this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
          }
      } */


        if (collision.gameObject.tag == "Block")
        {
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
        }

        /* if(!photonView.IsMine && collision.gameObject.tag == "Block")
           {
               this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);

           } */



        if (!photonView.IsMine)
        {
            return;
        }

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();


        if (target != null && (!target.IsMine || target.IsSceneView))
        {
            if (target.tag == "Player") //Check if our bullet hit the player
            {

                target.RPC("HealthUpdate", RpcTarget.AllBuffered, bulletDamage);  //whenever bullet hit someone we are going to update to health 
                           

                if (target.GetComponent<HealthBar>().health <= 0)
                {
                    this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
                    Player Gotkilled = target.Owner;
                    target.RPC("deathCounter", Gotkilled);
                    //target.Owner.NickName
                    target.RPC("KillCounter", localPlayerObj.GetComponent<PhotonView>().Owner,killerName);
                }
                
            }
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);

        }
    }

   










}
