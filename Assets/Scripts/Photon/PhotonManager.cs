using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    
    public static PhotonManager instance;

    private string roomName = "";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    #region Public methods
    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        //PhotonNetwork.JoinRandomRoom();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(0);
        Debug.Log("Player has been disconnected due to reasion : " + cause.ToString());
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open games available");
        //CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined the room");
        //SceneManager.LoadScene(1);
        if (PhotonNetwork.CurrentRoom.PlayerCount > 0)
        {
            Debug.Log("We load the 'Room for 1' ");
            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel(1);//PhotonNetwork.CurrentRoom.Name);
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed. Reason : "+returnCode+" -> "+message);
        //CreateRoom();
    }

    #endregion

    #region Private methods

    #endregion

    #region Public methods
    public void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        //int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 10
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }
    public void SetRoomName(string room){ roomName = room;}
    public string GetRoomName() { return roomName; }


    public GameObject InstantiateSceneGameObject(string prefabPath, Vector3 position, Quaternion rotation)
    {
        if(!PhotonNetwork.IsMasterClient)
            return null;
        return PhotonNetwork.InstantiateSceneObject(prefabPath, position, rotation);
    }

    public GameObject InstantiateGameobject(string prefabPath, Vector3 position, Quaternion rotation)
    {
        
        return PhotonNetwork.Instantiate(prefabPath, position, rotation);
    }

    public void DestroySceneGameObject(GameObject go)
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(go);
    }

    public void DestroyGameobject(GameObject go)
    {
        go.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
        PhotonNetwork.Destroy(go);
    }
    #endregion
}
