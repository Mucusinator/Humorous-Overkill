using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]public PlayerInfo m_plyInfo;
    void Start () {
        m_plyInfo = GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>().GetPlayerInfo;
    }

    public bool isHealthFull{
        get { return m_plyInfo.m_playerHealth == 100 ? true : false; }
    }
    public bool isDead {
        get { return m_plyInfo.m_playerHealth == 0 ? true : false; }
    }


    public void AddHealth (int health) {
        m_plyInfo.m_playerHealth += health;
        CheckHealth();
    }

    public void TakeDamage(int damage) {
        m_plyInfo.m_playerHealth -= damage;
        CheckHealth();
    }


    void CheckHealth () {
        if (m_plyInfo.m_playerHealth > 100)
            m_plyInfo.m_playerHealth = 100;
        else if (m_plyInfo.m_playerHealth < 0)
            m_plyInfo.m_playerHealth = 0;
    }
}
