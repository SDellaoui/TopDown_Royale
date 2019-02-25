using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhotonLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    #region Private fields
    string gameVersion = "1";

    private List<RoomInfo> roomList;
    #endregion

    #region Public fields
    
    public GameObject connectButton;
    public GameObject disconnectButton;
    public GameObject createButton;
    public GameObject joinButton;
    public GameObject cancelButton;

    public GameObject roomListUI;

    public Text textRoom;
    #endregion

    #region MonoBehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Public methods
    public override void OnConnectedToMaster()
    {
        
        createButton.SetActive(true);
        joinButton.SetActive(true);
        connectButton.SetActive(false);
        disconnectButton.SetActive(true);
        textRoom.transform.parent.gameObject.SetActive(true);
        return;
#if UNITY_EDITOR
        PhotonManager.instance.SetRoomName("room_test");
        PhotonManager.instance.CreateRoom();
#else
        PhotonNetwork.JoinRoom("room_test");
        //PhotonNetwork.JoinRandomRoom();
#endif

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectButton.SetActive(true);
        createButton.SetActive(false);
        joinButton.SetActive(false);
        cancelButton.SetActive(false);
        disconnectButton.SetActive(false);
        //Debug.Log("UI Lobby Disconnected");
    }

    public void OnCreateRoomButtonClicked()
    {
        //Debug.Log("Create Room Button clicked");
        if (textRoom.text == "")
            return;
        createButton.SetActive(false);
        joinButton.SetActive(false);
        cancelButton.SetActive(true);
        textRoom.transform.parent.gameObject.SetActive(false);
        PhotonManager.instance.CreateRoom();
    }
    public void OnJoinRoomButtonClicked()
    {
        PhotonNetwork.JoinLobby();
        createButton.SetActive(false);
        joinButton.SetActive(false);
        disconnectButton.SetActive(false);
        textRoom.transform.parent.gameObject.SetActive(false);
        roomListUI.SetActive(true);
        /*
        Debug.Log("Join Room Button clicked");
        if (textRoom.text == "")
            return;
        createButton.SetActive(false);
        joinButton.SetActive(false);
        cancelButton.SetActive(true);
        textRoom.transform.parent.gameObject.SetActive(false);
        PhotonNetwork.JoinRoom(PhotonManager.instance.GetRoomName());
        */
    }
    public void OnJoinRoomBackButtonClicked()
    {
        createButton.SetActive(true);
        joinButton.SetActive(true);
        cancelButton.SetActive(false);
        disconnectButton.SetActive(true);
        textRoom.transform.parent.gameObject.SetActive(true);
        roomListUI.SetActive(false);
    }
    public void OnConnectButtonClicked()
    {
        //Debug.Log("Button Offline clicked");
        PhotonNetwork.ConnectUsingSettings(); //Connects to Master photon server
    }
    public void OnDisconnectButtonClicked()
    {
        PhotonNetwork.Disconnect();
    }
    public void OnCancelButtonClicked()
    {
        //Debug.Log("Cancel button was clicked");
        cancelButton.SetActive(false);
        createButton.SetActive(true);
        joinButton.SetActive(true);
        textRoom.transform.parent.gameObject.SetActive(true);
        if(PhotonNetwork.CurrentRoom != null)
            PhotonNetwork.LeaveRoom();
    }
    public void OnRoomNameEndEdit()
    {
        PhotonManager.instance.SetRoomName(textRoom.text);
    }
    public void UpdateRoomList()
    {
        PhotonNetwork.JoinLobby();   
    }
    #endregion
}
