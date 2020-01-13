using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;


public class GameManager : MonoBehaviourPun
{

    public GameObject playerPrefab;
    public GameObject canvas;
    public GameObject sceneCam;
    public Text pingrate; //for setting pingrate
    public Text spawnTime;
    public GameObject respawnUI;    //for setActive respawnUI reference
    private float TimeAmount = 3f;
    private bool StartRespawnTime;
    [HideInInspector]
    public GameObject LocalPlayer;
    public GameObject LeaveScreen;

    //KillCounter ekranı için
    public GameObject KillGotKilledFeedBox;

    //for win and lose screen
    public GameObject winScreen;
    public Text wintext;
   
   



    public static GameManager reference = null;

    void Awake()  //After the game screen loaded ,spawn canvas will be shown
    {
        reference = this;
        canvas.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleLeaveScreen();
        }
        if (StartRespawnTime)
        {
            StartRespawn();
        }
        
        pingrate.text = "PingRate: " + PhotonNetwork.GetPing();
    }

    public void StartRespawn()
    {
        TimeAmount -= Time.deltaTime;
        spawnTime.text = "Respawn in: " + TimeAmount.ToString("F0");
        if (TimeAmount <= 0)
        {
            respawnUI.SetActive(false);
            StartRespawnTime = false;
            PlayerSpawnRandomLocation();



            LocalPlayer.GetComponent<HealthBar>().EnableInputs();
            LocalPlayer.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);
        }
    }

    public void ToggleLeaveScreen()
    {
        if (LeaveScreen.activeSelf)
        {
            LeaveScreen.SetActive(false);
        }
        else
        {
            LeaveScreen.SetActive(true);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }


    public void PlayerSpawnRandomLocation() //After dead randomly give location
    {
        float randomPosition = Random.Range(-6, 6);
        LocalPlayer.transform.localPosition = new Vector2(randomPosition, 2);
    }

    public void EnableRespawn() //We will call this method whenever player health reaches to 0
    {
        TimeAmount = 3;
        StartRespawnTime = true;
        respawnUI.SetActive(true);
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-6, 6);  //We need random value for spawning player in random position
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(playerPrefab.transform.position.x * randomValue, playerPrefab.transform.position.y*randomValue), Quaternion.identity, 0);   //We have to use PhotonNetwork Insantiate for the spawn player so other can able to see us.
        canvas.SetActive(false);
    }



    //WinScreen

    public void WinScreen(string name)
    {
        winScreen.SetActive(true);
        wintext.text =name;

    }

 





}
