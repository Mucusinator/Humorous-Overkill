using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    [SerializeField] public float m_sensitivity = 1f;
    [SerializeField] public float m_minimumAngle = -60f;
    [SerializeField] public float m_maximumAngle = 40f;

    private float m_rotation = 0;
    //private PlayerController m_pc;
    private Transform m_camera;
    private Transform m_transform;

    void Start () {
        //m_pc        = this.GetComponent<PlayerController>();
        m_camera    = GameObject.FindGameObjectWithTag("MainCamera").transform;
        m_transform = this.transform;
    }

    private void LateUpdate () {
        RotateCamera();
    }

    private void RotateCamera () {
        m_rotation -= Input.GetAxis("Mouse Y") * m_sensitivity;
        m_rotation = Mathf.Clamp(m_rotation, m_minimumAngle, m_maximumAngle);

        this.transform.localEulerAngles = new Vector3(m_rotation, this.transform.localEulerAngles.y, 0);
    }
}
