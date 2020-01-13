using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks //we are inherit Menu Manager from MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject UserNameScreen, ConnectScreen;

    [SerializeField]
    private GameObject CreateCharacterButton;

    [SerializeField]
    private InputField UserNameInput, CrateRoomInput, JoinRoomInput;



    void Awake()  //With this function we are going to connect with our photon server.
    {
        PhotonNetwork.ConnectUsingSettings();  //using our photon server settings to connect us to server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master!");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    //after succesfully connecting to a lobby photon will call for this method 
    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to Lobby!");
        //if we succesfully connected to lobby then setActive nameScreen 
        UserNameScreen.SetActive(true);
    }


    public override void OnJoinedRoom()   //after succesfully joined to room 
    {
        //play game scene
        PhotonNetwork.LoadLevel(1);  //Loading the gameplay scene 
    }




    #region UIMethods
    public void onClick_CreateNameButton()
    {
        PhotonNetwork.NickName = UserNameInput.text;
        UserNameScreen.SetActive(false);
        ConnectScreen.SetActive(true);
    }

    public void OnNameField_Changed()
    {
        if (UserNameInput.text.Length >= 2)
        {
            CreateCharacterButton.SetActive(true);
        }
        else
        {
            CreateCharacterButton.SetActive(false);
        }
    }

    public void Onclick_JoinRoom()
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4; //we are setting maximum player number to 4
        PhotonNetwork.JoinOrCreateRoom(JoinRoomInput.text,ro,TypedLobby.Default);

    }
    public void Onclick_CreateRoom()
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4; //we are setting maximum player number to 4
        PhotonNetwork.CreateRoom(CrateRoomInput.text, ro, null);
    }



    #endregion


}
