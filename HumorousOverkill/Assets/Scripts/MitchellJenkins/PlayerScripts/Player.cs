using UnityEngine;
using EventHandler;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour {

    public PlayerInfo m_ply;
    private DroneEnemyInfo m_droneInfo;
    private Animator m_animator;
    private CharacterController m_cc;
    private RuntimeAnimatorController m_animatorController;
    private Rigidbody m_rb;

    [SerializeField] private bool m_cameraEnabled = true;
    [SerializeField] private bool m_movementEnabled = true;

    public void Awake () {
        m_cc = this.GetComponent<CharacterController>() as CharacterController;
        EventManager<GameEvent>.Add(HandleEvent);       
    }
    

    public CharacterController _CharacterController {
        get { return this.m_cc; }
    }
    public Animator _Animator {
        get { return this.m_animator; }
    }
    public PlayerInfo _PlayerInfo {
        get { return m_ply; }
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
        if (m_ply.m_playerHealth > 100) m_ply.m_playerHealth = 100;
        else if (m_ply.m_playerHealth < 0) m_ply.m_playerHealth = 0;

        if (m_ply.m_playerHealth <= 0) {
            __event<GameEvent>.InvokeEvent(this, new __eArg<GameEvent>(GameEvent.STATE_LOSE_SCREEN, null, null, null));
        }
    }

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Projectile") {
            m_ply.m_playerHealth -= m_droneInfo.damage;
            CheckHealth();
        }
    }

    void Update () {
        EventManager<GameEvent>.InvokeGameState(this, null, m_ply.m_playerHealth / 100.0f, typeof(UIManager), GameEvent.UI_HEALTH);
    }

    public void HandleEvent (object s, __eArg<GameEvent> e) {
        if (s == (object)this) return;

        switch (e.arg) {
        case GameEvent._NULL_:
            if (e.type == typeof(PlayerManager)) {
                m_ply = (PlayerInfo)e.value;
            }
            if (e.type == typeof(EnemyManager)) {
                m_droneInfo = (DroneEnemyInfo)e.value;
            }
            break;
        case GameEvent.PLAYER_DAMAGE:
            m_ply.m_playerHealth -= (float)e.value;
            CheckHealth();
            break;
        }
    }
}
