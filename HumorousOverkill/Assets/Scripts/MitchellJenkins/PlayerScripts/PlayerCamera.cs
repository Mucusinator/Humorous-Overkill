using UnityEngine;

public class PlayerCamera : MonoBehaviour{

    [SerializeField] public float m_sensitivity = 1f;
    [SerializeField] public float m_minimumAngle = -60f;
    [SerializeField] public float m_maximumAngle = 40f;

    private PlayerInfo m_ply;

    private float m_rotation = 0;
    //private PlayerController m_pc;
    private Transform m_camera;
    private Transform m_transform;

    void Start () {
        //m_pc        = this.GetComponent<PlayerController>();
        m_camera    = GameObject.FindGameObjectWithTag("MainCamera").transform;
        m_transform = this.transform;
        m_ply       = this.GetComponentInParent<Player>()._PlayerInfo;
    }

    private void LateUpdate () {
        RotateCamera();
    }

    private void RotateCamera () {
        m_rotation -= Input.GetAxis("Mouse Y") * m_ply.m_cameraSensitivity;
        m_rotation = Mathf.Clamp(m_rotation, m_ply.m_cameraMinimumAngle, m_ply.m_cameraMaximumAngle);

        this.transform.localEulerAngles = new Vector3(m_rotation, this.transform.localEulerAngles.y, 0);
    }
}
