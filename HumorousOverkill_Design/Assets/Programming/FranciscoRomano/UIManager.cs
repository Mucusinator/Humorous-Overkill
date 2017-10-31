using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : EventHandler.EventHandle
{
    [Range(0,  1)] public float m_healthTEST = 1.0f;
    [Range(0, 99)] public float m_ammoCurTEST = 99.0f;
    [Range(0, 99)] public float m_ammoMaxTEST = 99.0f;

    public UIImage m_playerStatsHealth = new UIImage();
    public UIText m_playerStatsMaxAmmo = new UIText();
    public UIText m_playerStatsCurAmmo = new UIText();

    //public void OnValidate()
    //{
    //    if (m_playerStatsHealth == null) return;
    //    if (m_playerStatsCurAmmo == null) return;
    //    if (m_playerStatsMaxAmmo == null) return;
    //    if (m_playerStatsHealth.m_width == 0)   
    //    {
    //        m_playerStatsHealth.m_width = m_playerStatsHealth.m_image.rectTransform.rect.width;
    //        m_playerStatsHealth.m_height = m_playerStatsHealth.m_image.rectTransform.rect.height;
    //    }

    //    HandleEvent(GameEvent.UI_HEALTH, m_healthTEST);
    //    HandleEvent(GameEvent.UI_AMMO_CUR, m_ammoCurTEST);
    //    HandleEvent(GameEvent.UI_AMMO_MAX, m_ammoMaxTEST);
    //}

    public override bool HandleEvent(GameEvent e)
    {
        switch (e)
        {
            // handle Game states
            case GameEvent.GAME_STATE_MENU:
                break;
            case GameEvent.GAME_STATE_START:
                break;
            case GameEvent.GAME_STATE_PAUSE:
                break;
            case GameEvent.GAME_STATE_RESTART:
                break;
            case GameEvent.GAME_STATE_CONTINUE:
                break;
            // default handle
            default:
                base.HandleEvent(e);
                break;
        }
        return true;
    }

    public override bool HandleEvent(GameEvent e, float amount)
    {
        switch (e)
        {
            // handle UI states
            case GameEvent.UI_HEALTH:
                m_playerStatsHealth.UpdateWidth(amount);
                break;
            case GameEvent.UI_AMMO_CUR:
                m_playerStatsCurAmmo.Update((int)amount);
                break;
            case GameEvent.UI_AMMO_MAX:
                m_playerStatsMaxAmmo.Update((int)amount);
                break;
            // default handle
            default:
                base.HandleEvent(e);
                break;
        }
        return true;
    }

    [System.Serializable]
    public class UIText
    {
        // public variables
        public UnityEngine.UI.Text m_text = null;
        // public class functions
        public void Update(int value) { m_text.text = value.ToString(); }
        public void Update(float value) { m_text.text = value.ToString(); }
        public void Update(string value) { m_text.text = value; }
    }

    [System.Serializable]
    public class UIImage
    {
        // public variables
        public UnityEngine.UI.Image m_image = null;
        // private variables
        [HideInInspector]
        public float m_width = 0;
        [HideInInspector]
        public float m_height = 0;
        // public class functions
        public void UpdateSize(float deltaW, float deltaH) { m_image.rectTransform.sizeDelta = new Vector2(deltaW * m_width, deltaH * m_height); }
        public void UpdateWidth(float delta) { m_image.rectTransform.sizeDelta = new Vector2(delta * m_width, m_image.rectTransform.rect.height); }
        public void UpdateHeight(float delta) { m_image.rectTransform.sizeDelta = new Vector2(m_image.rectTransform.rect.width, delta * m_height); }
    }

}