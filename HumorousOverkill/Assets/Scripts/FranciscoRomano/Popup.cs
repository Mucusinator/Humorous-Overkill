using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Popup : MonoBehaviour
{
    private class FadeInformation
    {
        // :: variables
        public float speed = 1.0f;
        public float delay = 1.0f;
        public float curTime = 0.0f;
        public static float maximum = 1.0f;
        public static float minimum = 0.0f;
        // :: functions
        public void Update(UnityEngine.UI.Image source)
        {
            // check status
            if (!IsDelayComplete()) return;
            // update fade values
            Color color = source.color;
            float amount = color.a + speed * Time.deltaTime;
            color.a = Mathf.Clamp(amount, minimum, maximum);
            source.color = color;
        }
        public bool IsFadeComplete(UnityEngine.UI.Image source)
        {
            // check if fade finished
            return source.color.a == maximum || source.color.a == minimum;
        }
        public bool IsDelayComplete()
        {
            // check if delay finished
            return (Time.time - curTime) > delay;
        }
    }
    private class PopupFadeInformation
    {
        // :: variables
        public int index = 0;
        public Sprite sprite;
        public UnityEngine.UI.Image source = null;
        public FadeInformation[] fadeInNOut = new FadeInformation[2];
        // :: functions
        public void Begin()
        {
            // update sprite
            source.sprite = sprite;
            fadeInNOut[0].curTime = Time.time;
        }
        public void Update()
        {
            // check status
            if (IsComplete()) return;
            // update fade values
            fadeInNOut[index].Update(source);
            // check current fade status
            if (!fadeInNOut[index].IsDelayComplete()) return;
            if (!fadeInNOut[index].IsFadeComplete(source)) return;
            // check if fade infomation left
            index++;
            if ((fadeInNOut.Length - index) > 0)
            {
                // increment index
                fadeInNOut[index].curTime = Time.time;
            }
        }
        public bool IsComplete()
        {
            // check if fade finished
            return index == fadeInNOut.Length;
        }
    }
    // :: variables
    public bool active = true;
    public float fadeInDelay = 1.0f;
    public float fadeInSpeed = 1.0f;
    public float fadeOutDelay = 1.0f;
    public float fadeOutSpeed = 1.0f;
    public Sprite[] sprites = new Sprite[0];
    public UnityEngine.UI.Image source = null;
    private static Popup activeRunningPopup = null;
    private List<PopupFadeInformation> fadeList = new List<PopupFadeInformation>();
    // :: functions
    void Start()
    {
        EventManager<GameEvent>.Add(HandleMessage);
        source.color = new Color(1, 1, 1, 0);
    }
    void Update()
    {
        if (!active) return;
        if (fadeList.Count == 0) return;
        // update current fade
        fadeList[0].Update();
        // check if fade finished
        if (fadeList[0].IsComplete())
        {
            // remove current fade
            fadeList.RemoveAt(0);
            // check if list empty
            if (fadeList.Count > 0)
            {
                // begin fade
                fadeList[0].Begin();
            }
        }
    }
    private void HandleMessage(object sender, __eArg<GameEvent> e)
    {
        if (sender == (object)this) return;
        switch (e.arg)
        {
            case GameEvent.STATE_MENU:
            case GameEvent.STATE_PAUSE:
                active = false;
                break;
            case GameEvent.STATE_CONTINUE:
                if (fadeList.Count > 0) active = true;
                break;
        }        
    }

    public void OnTriggerEnter(Collider collider)
    {
        // check if player
        if (collider.tag == "Player")
        {
            if (!active) return;
            if (activeRunningPopup == this) return;
            // update previous popup
            if (activeRunningPopup != null)
            {
                foreach (PopupFadeInformation oldInfo in activeRunningPopup.fadeList)
                {
                    oldInfo.source = source;
                    fadeList.Add(oldInfo);
                }
                activeRunningPopup.active = false;
                activeRunningPopup.fadeList.Clear();
            }
            // update current popup
            activeRunningPopup = this;
            foreach (Sprite sprite in sprites)
            {
                PopupFadeInformation newInfo = new PopupFadeInformation();
                newInfo.fadeInNOut[0] = new FadeInformation();
                newInfo.fadeInNOut[1] = new FadeInformation();
                newInfo.fadeInNOut[0].curTime = Time.time;
                newInfo.fadeInNOut[0].delay = fadeInDelay;
                newInfo.fadeInNOut[1].delay = fadeOutDelay;
                newInfo.fadeInNOut[0].speed = 1 / fadeInSpeed;
                newInfo.fadeInNOut[1].speed = -1 / fadeOutSpeed;
                newInfo.source = source;
                newInfo.sprite = sprite;
                fadeList.Add(newInfo);
            }
            fadeList[0].Begin();
        }
    }
}