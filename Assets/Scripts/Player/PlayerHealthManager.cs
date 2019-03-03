using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealthManager : MonoBehaviourPunCallbacks,IPunObservable
{
    private int health;
    private int shield;
    private bool isAlive;

    //UI
    public Text healthUI;
    public Text shieldUI;

    // Start is called before the first frame update
    void Start()
    {
        shield = 70;
        health = 100;
        shieldUI.text = shield.ToString();
        healthUI.text = health.ToString(); 
    }
    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    #region Public Methods
    public int GetHealth() { return health; }
    public void SetHealth(int _health)
    {
        health = Mathf.Clamp(health + _health, 0, 100);
        healthUI.text = health.ToString();
    }
    public int GetShield() { return shield; }
    public void SetShield(int _shield)
    {
        shield = Mathf.Clamp(shield + _shield, 0, 100);
        shieldUI.text = shield.ToString();
    }

    public void SetDamage(int damageValue)
    {
        int previousShield = shield;

        shield = Mathf.Clamp(shield - damageValue, 0, 100);
        health = Mathf.Clamp(health - (damageValue - (previousShield - shield)), 0, 100);
        shieldUI.text = shield.ToString();
        healthUI.text = health.ToString();
        if (health == 0)
            isAlive = false;
    }
    #endregion

    #region RPC Events
    [PunRPC]
    void RPCSetDamage(int damage)
    {
        Debug.Log("Damage with "+damage);
        SetDamage(damage);
    }
    #endregion
}
