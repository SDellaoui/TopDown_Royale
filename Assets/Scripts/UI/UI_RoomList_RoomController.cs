using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomList_RoomController : MonoBehaviourPunCallbacks
{
    private int nbPlayersInRoom;
    private int roomMaxPlayers;

    public Text roomName;
    public Text roomPlayers;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitRoomInfo(string name, int nPlayers, int maxPlayers)
    {
        roomName.text = name;
        roomPlayers.text = $"({nPlayers}/{maxPlayers})";
    }

    #region Join Room Button
    public void OnJoinRoomButtonClicked()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }
    #endregion
}
