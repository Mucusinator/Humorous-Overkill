using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FranciscoRomano.UI.classes;
using System;

public class UIManager : EventHandler.EventHandle
{
    [Range(0,  1)] public float m_health = 1.0f;
    [Range(0, 99)] public float m_ammoCur = 99.0f;
    [Range(0, 99)] public float m_ammoMax = 99.0f;
    // This boolean is for when the UI manager nees to be tested. (Added by Zack.) 
    public bool DEBUG;


    // :: variables
    Dictionary<GameEvent, List<TextInfo>> textDictionary;
    Dictionary<GameEvent, List<ImageAnimationInfo>> imageAnimationDictionary;
    // :: functions
    void Start()
    {
        textDictionary = new Dictionary<GameEvent, List<TextInfo>>();
        imageAnimationDictionary = new Dictionary<GameEvent, List<ImageAnimationInfo>>();

        foreach (GameEvent e in Enum.GetValues(typeof(GameEvent)))
        {
            textDictionary.Add(e, new List<TextInfo>());
            imageAnimationDictionary.Add(e, new List<ImageAnimationInfo>());
        }

        foreach (UIProperty property in GetComponentsInChildren<UIProperty>())
        {
            switch (property.type)
            {
                case UIProperty.Type.TEXT:
                    textDictionary[property.triggerEvent].Add(TextInfo.Create(property));
                    break;
                case UIProperty.Type.IMAGE_ANIMATION:
                    imageAnimationDictionary[property.triggerEvent].Add(ImageAnimationInfo.Create(property));
                    break;
            }
        }
    }

    void Update()
    {
        if (DEBUG)
        {
            HandleEvent(GameEvent.UI_HEALTH, m_health);
            HandleEvent(GameEvent.UI_AMMO_CUR, m_ammoCur);
            HandleEvent(GameEvent.UI_AMMO_MAX, m_ammoMax);
        }
    }

    public override bool HandleEvent(GameEvent e, float amount)
    {
        switch (e)
        {
            case GameEvent.UI_HEALTH:
            case GameEvent.UI_AMMO_CUR:
            case GameEvent.UI_AMMO_MAX:
                if (textDictionary.ContainsKey(e))
                    foreach (TextInfo info in textDictionary[e])
                        info.Update(amount);
                if (imageAnimationDictionary.ContainsKey(e))
                    foreach (ImageAnimationInfo info in imageAnimationDictionary[e])
                        info.Update(amount);
                break;
        }
        return true;
    }

    //public Texture[] gif;

    //private UIImage m_playerStatsHealth = new UIImage();
    //private UIText m_playerStatsMaxAmmo = new UIText();
    //private UIText m_playerStatsCurAmmo = new UIText();


    //void Start()
    //{
    //    // A bit complex at this stage...
    //    // Will be comented properly and improved
    //    UIProperty[] properties = GetComponentsInChildren<UIProperty>();
    //    foreach(UIProperty property in properties)
    //    {
    //        switch(property.type)
    //        {
    //            case UIProperty.Type.TEXT:
    //                if (property.gameEvent == GameEvent.UI_AMMO_CUR) m_playerStatsCurAmmo.m_text = property.GetComponent<UnityEngine.UI.Text>();
    //                else if (property.gameEvent == GameEvent.UI_AMMO_MAX) m_playerStatsMaxAmmo.m_text = property.GetComponent<UnityEngine.UI.Text>();
    //                break;
    //            case UIProperty.Type.IMAGE_ANIMATION:

    //                break;
    //        }
    //    }
    //}

    //// ############################################ //
    //// ## FOR TESTING ONLY ######################## //
    //// ############################################ //
    //void Update()
    //{
    //    if (DEBUG)
    //    {
    //        HandleEvent(GameEvent.UI_HEALTH, m_healthTEST);
    //        HandleEvent(GameEvent.UI_AMMO_CUR, m_ammoCurTEST);
    //        HandleEvent(GameEvent.UI_AMMO_MAX, m_ammoMaxTEST);
    //    }

    //}
    //// ############################################ //
    //// ############################################ //
    //// ############################################ //

    //public override bool HandleEvent(GameEvent e)
    //{
    //    switch (e)
    //    {
    //        // handle Game states
    //        case GameEvent.STATE_MENU:
    //            break;
    //        case GameEvent.STATE_START:
    //            break;
    //        case GameEvent.STATE_PAUSE:
    //            break;
    //        case GameEvent.STATE_RESTART:
    //            break;
    //        case GameEvent.STATE_CONTINUE:
    //            break;
    //    }
    //    return true;
    //}

    //public override bool HandleEvent(GameEvent e, float amount)
    //{
    //    switch (e)
    //    {
    //        // handle UI states
    //        case GameEvent.UI_HEALTH:
    //            m_playerStatsHealth.UpdateWidth(amount);
    //            break;
    //        case GameEvent.UI_AMMO_CUR:
    //            m_playerStatsCurAmmo.Update((int)amount);
    //            break;
    //        case GameEvent.UI_AMMO_MAX:
    //            m_playerStatsMaxAmmo.Update((int)amount);
    //            break;
    //    }
    //    return true;
    //}

    //[System.Serializable]
    //public class UIText
    //{
    //    // public variables
    //    public UnityEngine.UI.Text m_text = null;
    //    // public class functions
    //    public void Update(int value) { m_text.text = value.ToString(); }
    //    public void Update(float value) { m_text.text = value.ToString(); }
    //    public void Update(string value) { m_text.text = value; }
    //}

    //[System.Serializable]
    //public class UIImage
    //{
    //    // public variables
    //    public UnityEngine.UI.Image m_image = null;
    //    // private variables
    //    [HideInInspector]
    //    public float m_width = 0;
    //    [HideInInspector]
    //    public float m_height = 0;
    //    // public class functions
    //    public void UpdateSize(float deltaW, float deltaH) { m_image.rectTransform.sizeDelta = new Vector2(deltaW * m_width, deltaH * m_height); }
    //    public void UpdateWidth(float delta) { m_image.rectTransform.sizeDelta = new Vector2(delta * m_width, m_image.rectTransform.rect.height); }
    //    public void UpdateHeight(float delta) { m_image.rectTransform.sizeDelta = new Vector2(m_image.rectTransform.rect.width, delta * m_height); }
    //}
}