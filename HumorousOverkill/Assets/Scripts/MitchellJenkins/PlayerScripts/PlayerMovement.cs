using UnityEngine;
using EventHandler;

[BindListener("PlayerManager", typeof(PlayerManager))]
public class PlayerMovement : EventHandle {

    private CharacterController m_cc;
    private PlayerCamera m_camera;
    [SerializeField] private PlayerInfo m_ply;
    private Transform m_transform;
    private Animator m_animator;
    
    public Vector3 m_moveDirection = Vector3.zero;
    private float m_horizontal = 0f;
    private float m_vertical = 0f;
    public float m_moveSpeed = 10f;
    private float m_gravity = 20f;
    private float m_jumpHeight = 10f;

    public LayerMask m_groundMask;
    public bool m_grounded = true;
    public bool m_isUnderObject = false;

    void Start () {
        m_camera    = this.GetComponentInChildren<PlayerCamera>();
        m_cc        = this.GetComponent<CharacterController>() as CharacterController;
        m_ply       = this.GetEventListener("PlayerManager").gameObject.GetComponent<PlayerManager>().GetPlayerInfo;
        m_transform = this.transform;
    }

    void Update () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.Rotate(0f, Input.GetAxis("Mouse X") * 200 * m_ply.m_cameraSensitivity * Time.deltaTime, 0f);

        m_moveDirection.x = Input.GetAxis("Horizontal") * m_moveSpeed;
        m_moveDirection.z = Input.GetAxis("Vertical") * m_moveSpeed;
        m_moveDirection = this.transform.TransformDirection(m_moveDirection);

        if (Input.GetKeyDown(KeyCode.Space) && m_grounded) { Jump(); }
        
        if (Input.GetKey(KeyCode.LeftControl) || m_isUnderObject) {
            Debug.DrawLine(this.transform.position, this.transform.position + Vector3.up * 2f, Color.cyan);
            if (Physics.Raycast(this.transform.position, Vector3.up, 2f, m_groundMask)) { m_isUnderObject = true; }
            else { m_isUnderObject = false; }
            if (m_cc.height > 1.1) { m_cc.height = Mathf.Lerp(m_cc.height, 1f, Time.deltaTime * 10f); } else { m_cc.height = 1f; }
            m_moveSpeed = m_ply.m_playerCrouchSpeed;
        } else {
            if (m_cc.height < 1.9) { m_cc.height = Mathf.Lerp(m_cc.height, 2f, Time.deltaTime * 10f); } else { m_cc.height = 2f; }
            if (Input.GetKey(KeyCode.LeftShift)) { m_moveSpeed = m_ply.m_playerRunSpeed; } else { m_moveSpeed = m_ply.m_playerWalkSpeed; }
        }

        Debug.DrawLine(this.transform.position + Vector3.down, this.transform.position + Vector3.down * 1.3f, Color.cyan);
        if (Physics.Raycast(this.transform.position + Vector3.down, Vector3.down, 0.3f, m_groundMask )) { m_grounded = true; }
        else {
            m_grounded = false;
            m_moveDirection.y -= m_gravity * Time.deltaTime;
        }

        m_cc.Move(m_moveDirection * Time.deltaTime);
    }

    private void Jump () {
        m_moveDirection.y = m_jumpHeight;
    }
}
