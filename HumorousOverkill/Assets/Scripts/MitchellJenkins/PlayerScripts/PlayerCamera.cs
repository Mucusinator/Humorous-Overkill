using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    [SerializeField] private float m_sensitivity = 5f;
    [SerializeField] private float m_minimumAngle = -60f;
    [SerializeField] private float m_maximumAngle = 40f;

    private float m_rotation = 0;
    private PlayerController m_pc;
    private Transform m_camera;
    private Transform m_transform;

    void Start () {
        m_pc        = this.GetComponent<PlayerController>();
        m_camera    = GameObject.FindGameObjectWithTag("MainCanera").transform;
        m_transform = this.transform;
    }

}
