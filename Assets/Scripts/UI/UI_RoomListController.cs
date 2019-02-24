using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class UI_RoomListController : MonoBehaviourPunCallbacks, ILobbyCallbacks
{

    public GameObject contentList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform child in contentList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (RoomInfo room in roomList)
        {
            GameObject r = Instantiate(Resources.Load("Prefabs/UI/UI_RoomList_Room")) as GameObject;
            r.GetComponent<UI_RoomList_RoomController>().InitRoomInfo(room.Name, room.PlayerCount, room.MaxPlayers);
            r.transform.SetParent(contentList.transform);
            Debug.Log(room.Name + " (" + room.PlayerCount + "/" + room.MaxPlayers + ")");
        }
    }
}
