﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class ChestController : MonoBehaviourPun, IPunObservable
{
    public GameObject[] Loots;

    private List<int> rpcLootIndexes;
    private int currentLootIndex;
    private int nLoots;
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        nLoots = Random.Range(1, 10);
        rpcLootIndexes = new List<int>();
        for (int i = 0; i < nLoots; i++)
        {
            rpcLootIndexes.Add(Random.Range(0, Loots.Length));
        }
        /*
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("RPCSetAll", RpcTarget.All, nLoots, rpcLootIndexes);
        */
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(nLoots);
            //stream.SendNext(rpcLootIndexes);
        }
        else
        {
            nLoots = (int)stream.ReceiveNext();
            //rpcLootIndexes = (List<int>)stream.ReceiveNext();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Projectile")
        {
            photonView.RPC("RPCDestroyChest", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    void SetAll(int _nLoots, List<int> _rpcLootIndexes)
    {
        nLoots = _nLoots;
        rpcLootIndexes = _rpcLootIndexes;
    }

    [PunRPC]
    void RPCDestroyChest()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        if(PhotonNetwork.IsMasterClient)
            StartCoroutine(SpawnLoots(Loots, transform.position));
    }
    
    IEnumerator SpawnLoots(GameObject[] loots, Vector3 position)
    {
        foreach(int i in rpcLootIndexes)
        {
            string prefabPath = "";
            switch(Loots[i].name)
            {
                case "Potion":
                    prefabPath = "Prefabs/Collectibles/Potion";
                    break;
                default:
                    break;
            }
            Debug.Log("collectible to spawn : " + prefabPath);
            if(prefabPath != "")
            {
                PhotonNetwork.Instantiate(prefabPath, transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(0.08f);
        }
        Destroy(gameObject);
    }
    /*
[Command]
void CmdDestroyChest(GameObject player)
{
   if (Loots.Length == 0)
       return;

   GetComponent<SpriteRenderer>().enabled = false;
   GetComponent<BoxCollider2D>().enabled = false;
   StartCoroutine(SpawnWithDelay(Loots, transform.position, player));
}

IEnumerator SpawnWithDelay(GameObject[] loots, Vector3 position, GameObject owner)
{

   for (int i = 0; i < Random.Range(5, 10); i++)
   {
       currentLootIndex = Random.Range(0, loots.Length);
       GameObject loot = Instantiate(Loots[currentLootIndex]) as GameObject;
       loot.transform.position = transform.position;

       //loot.GetComponent<CollectibleSpawnCurveController>().InitCurve(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
       NetworkServer.Spawn(loot);
       yield return new WaitForSeconds(0.08f);
   }
   Debug.Log("Unspawn Chest");
   NetworkServer.Destroy(gameObject);
}
*/
}
