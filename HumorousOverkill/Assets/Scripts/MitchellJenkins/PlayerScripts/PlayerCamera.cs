using UnityEngine;
using EventHandler;

public class PlayerCamera : MonoBehaviour
{
    [HideInInspector]
    public PlayerInfo m_ply;
    private float m_rotation = 0;
    void Awake()
    {
        EventManager<GameEvent>.Add(HandleMessage);
    }

    private void LateUpdate()
    {
        RotateCamera();
    }

    public void HandleMessage(object s, __eArg<GameEvent> e)
    {
        if (s == (object)this) return;
        switch (e.arg)
        {
            case GameEvent._NULL_:
                if (e.type == typeof(PlayerManager))
                {
                    m_ply = (PlayerInfo)e.value;
                }
                break;
        }
    }

    private void RotateCamera()
    {
        m_rotation -= Input.GetAxis("Mouse Y") * m_ply.m_cameraSensitivity * 2;
        m_rotation = Mathf.Clamp(m_rotation, m_ply.m_cameraMinimumAngle, m_ply.m_cameraMaximumAngle);

        this.transform.localEulerAngles = new Vector3(m_rotation, this.transform.localEulerAngles.y, 0);
    }
}
