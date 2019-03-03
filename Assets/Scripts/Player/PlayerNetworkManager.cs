using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNetworkManager : MonoBehaviourPunCallbacks, IPunObservable
{

    public static GameObject localPlayerInstance;

    public MonoBehaviour[] localScripts;
    public GameObject[] localGameObjects;

    public PlayerHealthManager playerHealthManager;
    public SpriteRenderer playerSpriteRenderer;
    bool spriteFlip = false;

    Rigidbody2D rb;
    Vector3 networkPosition;
    Vector2 networkVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //Physics2D.IgnoreLayerCollision(10, 8);
        if (photonView.IsMine)
        {
            Debug.Log("player spawned is mine");
            localPlayerInstance = this.gameObject;
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
            //Camera.main.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("player spawned is not mine");
            rb.bodyType = RigidbodyType2D.Static;
            //Player is Remote, deactivate the scripts and object that should only be enabled for the local player
            for (int i = 0; i < localScripts.Length; i++)
            {
                localScripts[i].enabled = false;
            }
            for (int i = 0; i < localGameObjects.Length; i++)
            {
                //localGameObjects[i].SetActive(false);
                if(localGameObjects[i].name == "Camera")
                {
                    localGameObjects[i].GetComponent<Camera>().enabled = false;
                    localGameObjects[i].GetComponent<AudioListener>().enabled = false;
                }
            }
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {
        
        if(!photonView.IsMine)
        {
            //transform.position = Vector3.MoveTowards(transform.position, networkPosition, Time.fixedDeltaTime);
            playerSpriteRenderer.flipX = spriteFlip;
            //rb.velocity = networkVelocity;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.IsWriting)
        {
            stream.SendNext(playerSpriteRenderer.flipX);
        }
        else
        {
            spriteFlip = (bool)stream.ReceiveNext();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        switch (collision.gameObject.tag)
        {
            case "Projectile":
                ProjectileController projectile = collision.gameObject.GetComponent<ProjectileController>();
                if (projectile.GetOwnerID() == photonView.Owner.UserId)
                    return;
                if (!photonView.IsMine)
                {
                    collision.gameObject.SetActive(false);
                }
                else
                {
                    photonView.RPC("RPCSetDamage", RpcTarget.All, projectile.damage);// collision.gameObject.GetPhotonView().InstantiationId, projectile.damage);
                }
                break;
            case "Collectible":
                CollectItem(collision.gameObject);
                break;
            default:
                break;
        }
    }
    public void SpawnGameObject(GameObject go, Vector3 position, Quaternion rotation)
    {
        photonView.RPC("RPCSpawnGameObject",RpcTarget.All, go, position, rotation, this.gameObject);
    }

    public void CollectItem(GameObject item)
    {
        CollectibleController collController = item.GetComponent<CollectibleController>();
        if (collController != null)
        {
            switch(collController.collectibleType.ToString())
            {
                case "Health":
                    if (playerHealthManager.GetHealth() == 100)
                        return;
                    break;
                default:
                    break;
            }
            item.SetActive(false);
            photonView.RPC("RPCCollectItem", RpcTarget.AllViaServer, collController.collectibleType.ToString(), collController.photonView.ViewID, photonView.ViewID);
        }
    }

    #region RPC FUNCTIONS
    [PunRPC]
    void ReceiveHealth(int health)
    {

    }

    [PunRPC]
    void RSpawnGameObject(GameObject go, Vector3 position, Quaternion rotation, GameObject owner)
    {
        Debug.Log("Spawn gameobject");
        GameObject g = Instantiate(go, position, rotation);
        switch(g.GetComponent<GameObjectSpawnableController>().Get())
        {
            case GameObjectSpawnableType.PROJECTILE:
                g.GetComponent<ProjectileController>().SetOwner(owner);
                break;
            case GameObjectSpawnableType.ITEM:
                break;
            default:
                break;
        }
    }

    [PunRPC]
    void RPCCollectItem(string itemType, int itemNetId, int ownerNetID)
    {
        PhotonView itemView = PhotonView.Find(itemNetId);
        if (itemView == null)
            return;
        CollectibleController item = itemView.gameObject.GetComponent<CollectibleController>();
        
        if ((item.ownerID != -1 && item.ownerID != photonView.ViewID))
            return;
        else
        {
            item.ownerID = photonView.ViewID;
            switch(itemType)
            {
                case "Health":
                    playerHealthManager.SetHealth(item.collectibleValue);
                    break;
                default:
                    break;
            }
            PhotonNetwork.Destroy(PhotonView.Find(itemNetId));
        }
    }
    #endregion
}
