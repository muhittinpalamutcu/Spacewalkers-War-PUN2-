using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class SpaceMan : MonoBehaviourPun
{
    public float MoveSpeed = 1f;
    public SpriteRenderer sprite;  //Our character able to turn left or right so we need spriterenderer to flip our character.
    public Animator anim;
    public PhotonView photonview;
    public GameObject BulletPrefab;
    public Transform BulletSpawnPointLeft;
    public Transform BulletSpawnPointRight;
    public Text PlayerName;
    public bool DisableInput = false;
    private Rigidbody2D rb;
 
    

    public string MyName;
 

    void Awake()
    {


        if (photonView.IsMine)
        {
            GameManager.reference.LocalPlayer = this.gameObject;
            PlayerName.text = "You :" + PhotonNetwork.NickName;
            PlayerName.color = Color.white;
            //for killerCount
            MyName = PhotonNetwork.NickName;
       

        }
        else
        {
            PlayerName.text = photonview.Owner.NickName;  //If it's not our view then other's view so 
            PlayerName.color = Color.red;
            

        }

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }


    //writing there update;
    void Update()
    {
        if (photonView.IsMine && !DisableInput)
        {
            checkInputs();
            rb.gravityScale = 0;
        }

    }

    private void checkInputs()
    {
        var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += movement * MoveSpeed * Time.deltaTime;

        //for shootings
        if (Input.GetMouseButtonDown(0))
        {

            /*  if (Input.GetKey(KeyCode.W))
              {
                  GameObject bullet = PhotonNetwork.Instantiate(BulletPrefab.name, new Vector2(0, BulletSpawnPointTop.position.y), Quaternion.identity, 0);
              }
              if (Input.GetKey(KeyCode.S))
              {
                  GameObject bullet = PhotonNetwork.Instantiate(BulletPrefab.name, BulletSpawnPointBottom.position, BulletSpawnPointBottom.rotation);
                  //  GameObject bullet = PhotonNetwork.Instantiate(BulletPrefab.name, new Vector2(0, BulletSpawnPointBottom.position.y), Quaternion.identity, 0);
              } */


            shot();
            anim.SetBool("IsBack", false);
            anim.SetBool("IsFront", false);


        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Checking animation
            anim.SetBool("IsMove", true);
            // sprite.flipX = true; this just flip our character in local instance not in photonNetwork for flip it and sycn it across the network we are going to use RPC
            photonview.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("IsMove", false);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);

            anim.SetBool("IsBack", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {

            anim.SetBool("IsBack", false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * MoveSpeed * Time.deltaTime);

        }
        if (Input.GetKeyDown(KeyCode.S))
        {

            anim.SetBool("IsFront", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {

            anim.SetBool("IsFront", false);
        }



        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetBool("IsMove", true);
            //Whenever I need to execute rpc I need to references of photonview
            photonview.RPC("FlipSprite_Left", RpcTarget.AllBuffered);


        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("IsMove", false);
        }


    }

    private void shot()
    {
        //We instantiate the bullet with transform object bullet spawn point 

        if (sprite.flipX == false)
        {
            GameObject bullet = PhotonNetwork.Instantiate(BulletPrefab.name, new Vector2(BulletSpawnPointLeft.position.x, BulletSpawnPointLeft.position.y), Quaternion.identity, 0);
            //for determine  
            bullet.GetComponent<Bullet>().localPlayerObj = this.gameObject;
        }

        if (sprite.flipX == true)
        {
            GameObject bullet = PhotonNetwork.Instantiate(BulletPrefab.name, new Vector2(BulletSpawnPointRight.position.x, BulletSpawnPointRight.position.y), Quaternion.identity, 0);
            //for determine 
            bullet.GetComponent<Bullet>().localPlayerObj = this.gameObject;
            bullet.GetComponent<PhotonView>().RPC("ChangeDirection", RpcTarget.AllBuffered);
        }

    }

    [PunRPC]
    private void FlipSprite_Right()
    {
        sprite.flipX = true;
    }

    [PunRPC]
    private void FlipSprite_Left()
    {
        sprite.flipX = false;
    }









}
