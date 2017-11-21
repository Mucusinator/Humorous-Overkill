using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIManager : EventHandler.EventHandle
{
    public bool DEBUG;
    public bool m_stateMenu = false;
    public bool m_statePause = false;
    public bool m_stateStart = false;
    [Range(0,  1)] public float m_health = 1.0f;
    [Range(0, 99)] public float m_ammoCur = 99.0f;
    [Range(0, 99)] public float m_ammoMax = 99.0f;
    // This boolean is for when the UI manager nees to be tested. (Added by Zack.) 

    // :: variables
    //List<UIProperty> propertyList = new List<UIProperty>();
    Dictionary<GameEvent, List<UIProperty>> propertyDictionary = new Dictionary<GameEvent, List<UIProperty>>();

    // :: functions
    void Start()
    {
        EventManager<GameEvent>.Add(HandleMessage);
        foreach (GameEvent e in Enum.GetValues(typeof(GameEvent)))
        {
            propertyDictionary.Add(e, new List<UIProperty>());
        }

        foreach (UIProperty property in GetComponentsInChildren<UIProperty>())
        {
            if (property.type == UIProperty.Type.NONE)
            {
                property.gameObject.SetActive(false);
            }
            propertyDictionary[property.triggerEvent].Add(property);
        }
    }

    void Update()
    {
        if (DEBUG)
        {
            HandleEvent(GameEvent.UI_HEALTH, m_health);
            HandleEvent(GameEvent.UI_AMMO_CUR, m_ammoCur);
            HandleEvent(GameEvent.UI_AMMO_MAX, m_ammoMax);
            if (m_stateMenu)
            {
                HandleEvent(GameEvent.STATE_MENU);
            }
            if (m_statePause)
            {
                HandleEvent(GameEvent.STATE_PAUSE);
            }
            if (m_stateStart)
            {
                HandleEvent(GameEvent.STATE_START);
            }
        }
        m_stateMenu = false;
        m_statePause = false;
        m_stateStart = false;
    }

    public void HandleMessage(object obj, __eArg<GameEvent> e)
    {
        if (obj == (object)this) return;
        switch (e.arg)
        {
            case GameEvent.UI_HEALTH:
            case GameEvent.UI_AMMO_CUR:
            case GameEvent.UI_AMMO_MAX:
                if (propertyDictionary.ContainsKey(e.arg))
                {
                    foreach (UIProperty property in propertyDictionary[e.arg])
                    {
                        property.UpdateComponent(e.arg != GameEvent.UI_HEALTH ? Mathf.Floor((float)e.value) : (float)e.value);
                    }
                }
                break;
            // handle Game States
            case GameEvent.STATE_MENU:
            case GameEvent.STATE_START:
            case GameEvent.STATE_PAUSE:
            case GameEvent.STATE_RESTART:
            case GameEvent.STATE_CONTINUE:
                foreach (GameEvent otherE in Enum.GetValues(typeof(GameEvent)))
                {
                    foreach (UIProperty property in propertyDictionary[otherE])
                    {
                        property.gameObject.SetActive(otherE == e.arg);
                    }
                }
                //foreach (UIProperty property in propertyList)
                //{
                //    property.gameObject.SetActive(false);
                //}
                //propertyList.Clear();
                //if (propertyDictionary.ContainsKey(e.arg))
                //{
                //    foreach (UIProperty property in propertyDictionary[e.arg])
                //    {
                //        property.gameObject.SetActive(true);
                //        propertyList.Add(property);
                //    }
                //}
                break;
        }
    }

    public override bool HandleEvent(GameEvent e)
    {
        HandleMessage(null, new __eArg<GameEvent>(e, this, null, typeof(UIManager)));
        return true;
    }

    public override bool HandleEvent(GameEvent e, float amount)
    {
        HandleMessage(null, new __eArg<GameEvent>(e, this, amount, typeof(UIManager)));
        return true;
    }

    //public void SuperCoolEventHanbdler(object s, __eArg<GameEvent> e)
    //{
    //    if (s != (System.Object)this)
    //    {
    //        switch (e.arg)
    //        {
    //            case GameEvent.UI_HEALTH:
    //                __event<GameEvent>.InvokeEvent(this, new __eArg<GameEvent>(GameEvent.STATE_CONTINUE, null, null, null));
    //                break;

    //        }   
    //    }
    //}

}