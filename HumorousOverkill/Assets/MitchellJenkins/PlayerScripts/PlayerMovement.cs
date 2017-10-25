using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private CharacterController m_cc;
    private PlayerInfo m_ply;
    private Transform m_transform;
    private Animator m_animator;
    private Camera m_camera;

    private Vector3 _moveDirection = Vector3.zero;
    private float m_horizontal = 0f;
    private float m_vertical = 0f;
    private float m_moveSpeed;
    private float m_airV = 0f;

    void Start () {
        m_camera    = this.GetComponent<Camera>();
        m_cc        = this.GetComponent<Player>()._CharacterController;
        m_ply       = this.GetComponent<Player>()._PlayerInfo;
        m_animator  = this.GetComponent<Player>()._Animator;
        m_transform = this.transform;
    }

    void Update () {

    }
}
