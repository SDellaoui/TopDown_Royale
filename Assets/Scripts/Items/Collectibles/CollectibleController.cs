using UnityEngine;

public enum CollectibleType
{
    Money,
    Health
}

public class CollectibleController : MonoBehaviour
{
    // Start is called before the first frame update
    public CollectibleType collectibleType;
    public int collectibleValue;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGO = collision.gameObject;
    }
    /*
    [Command]
    void CmdSendItemToPlayer(GameObject collisionGO)
    {
        if (collisionGO.tag == "Player")
        {
            switch (collectibleType)
            {
                case CollectibleType.Health:
                    if (collisionGO.GetComponent<PlayerController>().GetHealth() < 100)
                    {
                        collisionGO.GetComponent<PlayerNetworkSetup>().CmdAddHealth(collectibleValue);
                        NetworkServer.Destroy(gameObject);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    */
}
