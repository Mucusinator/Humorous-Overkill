using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {

    private PlayerInfo m_ply;
    private CharacterController m_cc;
    private Rigidbody m_rb;

    void Start () {
        m_ply = GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>().GetPlayerInfo;
        m_cc = this.GetComponent<CharacterController>() as CharacterController;
        m_rb = this.GetComponent<Rigidbody>() as Rigidbody;
    }

    public bool isHealthFull{
        get { return m_ply.m_playerHealth == 100 ? true : false; }
    }
    public bool isDead {
        get { return m_ply.m_playerHealth == 0 ? true : false; }
    }
    public void AddHealth (int health) {
        m_ply.m_playerHealth += health; CheckHealth();
    }
    public void TakeDamage(int damage) {
        m_ply.m_playerHealth -= damage; CheckHealth();
    }
    
    void CheckHealth () {
        if (m_plyInfo.m_playerHealth > 100) m_plyInfo.m_playerHealth = 100;
        else if (m_plyInfo.m_playerHealth < 0) m_plyInfo.m_playerHealth = 0;
    }

    void FixedUpdate () {
        
    }
}
