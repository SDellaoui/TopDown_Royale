using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public enum ItemType {Chest };
public class ItemNetWorkSpawner: MonoBehaviour
{
    public ItemType itemType;
    // Start is called before the first frame update
    void Start()
    {
        switch (itemType)
        {
            case ItemType.Chest:
                PhotonManager.instance.InstantiateSceneGameObject("Prefabs/Items/Chest", transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
