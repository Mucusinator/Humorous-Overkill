using UnityEngine;
using EventHandler;
using UnityEngine.SceneManagement;

[BindListener("PlayerManager", typeof(PlayerManager))]
[BindListener("UI", typeof(UIManager))]
[BindListener("EnemyManager", typeof(EnemyManager))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerMovement))]
public class Player : EventHandle {

    public PlayerInfo m_ply;
    private Animator m_animator;
    private CharacterController m_cc;
    private RuntimeAnimatorController m_animatorController;
    private Rigidbody m_rb;

    [SerializeField] private bool m_cameraEnabled = true;
    [SerializeField] private bool m_movementEnabled = true;

    override public void Awake () {
        base.Awake();
        m_ply = GetEventListener("PlayerManager").gameObject.GetComponent<PlayerManager>().GetPlayerInfo;
        m_cc = this.GetComponent<CharacterController>() as CharacterController;

        // m_animator.runtimeAnimatorController = m_animatorController;
        // m_cc.center = new Vector3(0f, 1f, 0f);
        // m_cc.height = 1.8f;
    }

    public void Start () {
        __event<MapState>.InvokeEvent(
            this,
            new __eArg<MapState>(
                MapState.PING,
                null,
                this.gameObject.transform,
                typeof(Player) ));
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Projectile") {
            m_ply.m_playerHealth -= GetEventListener("EnemyManager").gameObject.GetComponent<EnemyManager>().defaultDroneInfo.damage;
            CheckHealth();
        }
    }

    void Update () {
        GetEventListener("UI").HandleEvent(GameEvent.UI_HEALTH, m_ply.m_playerHealth / 100);
    }

    public override bool HandleEvent (GameEvent e, float value) {
        switch (e) {
        case GameEvent.PLAYER_DAMAGE:
            m_ply.m_playerHealth -= value;
            CheckHealth();
            break;
        }
        return true;
    }
}
