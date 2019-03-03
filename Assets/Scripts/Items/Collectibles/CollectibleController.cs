using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum CollectibleType
{
    Money,
    Health
}

public class CollectibleController : MonoBehaviourPunCallbacks, IPunObservable
{
    // Start is called before the first frame update
    public CollectibleType collectibleType;
    public int collectibleValue;

    public bool hasPwner = false;
    public int ownerID = -1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(ownerID);
        }
        else
        {
            ownerID = (int)stream.ReceiveNext();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if(collision.gameObject.tag == "Player")
        {
            ownerID = collision.gameObject.GetComponent<PlayerNetworkManager>().photonView.ViewID;
        }
    }
}
