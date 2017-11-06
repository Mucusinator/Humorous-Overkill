using UnityEngine;
using EventHandler;

[BindListener("PlayerManager", typeof(PlayerManager))]
public class PlayerCamera : EventHandle {
    private PlayerInfo m_ply;
    private float m_rotation = 0;
    void Start () {
        m_ply = GetEventListener("PlayerManager").gameObject.GetComponent<PlayerManager>().GetPlayerInfo;
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
