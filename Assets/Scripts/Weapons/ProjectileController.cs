using System.Linq;
using UnityEngine;
using Photon.Pun;


public class ProjectileController : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    private float speed = 8f;

    private GameObject owner;
    private bool isMine = true;
    private string ownerID = "";
    private bool stopped = false;
    private string[] destroyColliders = { "Wall", "Breakable"};
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stopped)
            return;
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
        //transform.position += transform.up * speed;
        Debug.DrawLine(transform.position, transform.position + transform.up);


    }
    public void SetBulletSpeed(float _speed)
    {
        speed = _speed;
    }

    public GameObject GetOwner() { return owner; }
    public void SetOwner(GameObject go)
    {
        owner = go;
    }
    public bool IsMyProjectile() { return isMine; }
    public string GetOwnerID() { return ownerID; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(destroyColliders.Contains(collision.collider.gameObject.tag))
        {
            Instantiate(Resources.Load("Prefabs/Particles/VFX_Bullet_Impact"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            info.photonView.RequestOwnership();
        }
        isMine = info.photonView.IsMine;
        ownerID = info.photonView.Owner.UserId;
        return;
        if (isMine)
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameManager.instance.playerPrefab.GetComponent<Collider2D>());
        //Debug.Log("Bullet spawned. Is mine ? -> " + isMine);
    }
}
