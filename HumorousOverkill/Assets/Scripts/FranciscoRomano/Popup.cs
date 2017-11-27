using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Popup : MonoBehaviour
{
    private class FadeInformation
    {
        // :: variables
        public float speed;
        public UnityEngine.UI.Image source;
        public static float maximum = 1.0f;
        public static float minimum = 0.0f;
        // :: functions
        public bool Update(float speed)
        {
            Color color = source.color;
            color.a += speed * Time.deltaTime;
            // calculate clamp
            float clamp = Mathf.Clamp(color.a, minimum, maximum);
            // check if volume is out of range
            if (color.a != clamp)
            {
                // animation complete
                color.a = clamp;
                source.color = color;
                return false;
            }
            // animation running
            source.color = color;
            return true;
        }
    }
    private class PopupFadeInformation
    {
        // :: variables
        public Sprite image;
        public float fadeInDelay;
        public float fadeInSpeed;
        public float fadeOutDelay;
        public float fadeOutSpeed;
        public FadeInformation fadeIn;
        public FadeInformation fadeOut;
        // :: functions
        public bool Update()
        {
            bool check = false;
            // update fade in
            if (!check && fadeIn != null)
            {
                check = fadeIn.Update(fadeInSpeed);
                if (!check) fadeIn = null;
            }
            // update fade out
            if (!check && fadeOut != null)
            {
                check = fadeOut.Update(fadeOutSpeed);
                if (!check) fadeOut = null;
            }
            // return result
            return check;
        }
    }
    // :: variables
    public bool enabled = false;
    public bool running = false;
    public float fadeInDelay = 1.0f;
    public float fadeInSpeed = 1.0f;
    public float fadeOutDelay = 1.0f;
    public float fadeOutSpeed = 1.0f;
    public Sprite[] sprites = new Sprite[0];
    private static List<PopupFadeInformation> fadeGroup = new List<PopupFadeInformation>();
    // :: functions
    void Start()
    {
        EventManager<GameEvent>.Add(HandleMessage);
    }
    void Update()
    {
        if (!enabled) return;
        if (!running) return;
        if (fadeGroup.Count == 0) return;
        if (!fadeGroup[0].Update())
        {
            fadeGroup.RemoveAt(0);
        }
    }
    private void HandleMessage(object sender, __eArg<GameEvent> e)
    {
        if (sender == (object)this) return;
        switch (e.arg)
        {
            case GameEvent.STATE_MENU:
            case GameEvent.STATE_PAUSE:
                enabled = false;
                break;
            case GameEvent.STATE_CONTINUE:
                enabled = true;
                break;
        }        
    }

    //public void OnTriggerExit()
    //public void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.tag == "Player")
    //    {
    //        if (running) return;
    //        if (!running)
    //        {
    //            ResetAll();
    //            running = true;
    //        }
    //        currentTime = Time.time;
    //    }
    //}

    //   [HideInInspector]
    //   public int index = 0;
    //   [HideInInspector]
    //   public bool fadeIn = true;
    //   [HideInInspector]
    //   public bool running = false;
    //   public float fadeInDelay = 1.0f;
    //   public float fadeInSpeed = 1.0f;
    //   public float fadeOutDelay = 1.0f;
    //   public float fadeOutSpeed = 1.0f;
    //   public Sprite[] sprites;
    //   public UnityEngine.UI.Image image;
    //   private bool complete = false;
    //   private float currentTime = 0;
    //   //private static Po

    //   void Start ()
    //   {
    //       EventManager<GameEvent>.Add(HandleMessage);
    //       ResetAll();
    //}

    //   void Update()
    //   {
    //       if (!running) return;
    //       if (fadeIn)
    //       {
    //           if (image.color.a < 1)
    //           {
    //               Color color1 = image.color;
    //               color1.a = Mathf.Min(color1.a + (Time.deltaTime / fadeInSpeed), 1);
    //               image.color = color1;
    //               currentTime = Time.time;
    //           }
    //           else if ((currentTime + fadeOutDelay) < Time.time)
    //           {
    //               fadeIn = false;
    //           };
    //       }
    //       else
    //       {
    //           if (image.color.a > 0)
    //           {
    //               Color color1 = image.color;
    //               color1.a = Mathf.Max(color1.a - (Time.deltaTime / fadeOutSpeed), 0);
    //               image.color = color1;
    //               currentTime = Time.time;
    //           }
    //           else if ((currentTime + fadeInDelay) < Time.time)
    //           {
    //               fadeIn = true;
    //               NextSprite();
    //           }
    //       }
    //   }

    //   void ResetAll()
    //   {
    //       index = 0;
    //       fadeIn = true;
    //       running = false;
    //       complete = false;
    //       image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    //       image.sprite = sprites[index++];
    //   }

    //   void NextSprite()
    //   {
    //       if (index < sprites.Length)
    //       {
    //           image.sprite = sprites[index++];
    //       }
    //       else
    //       {
    //           running = false;
    //       }
    //   }

    //   public void HandleMessage(object sender, __eArg<GameEvent> e)
    //   {
    //       if (sender == (object)this) return;
    //       switch (e.arg)
    //       {
    //           case GameEvent.STATE_START:
    //           case GameEvent.STATE_RESTART:
    //               ResetAll();
    //               break;
    //       }        
    //   }

    //   public void OnTriggerExit(Collider collider)
    //   {
    //       if (collider.tag == "Player")
    //       {
    //           running = false;
    //       }
    //   }

    //   public void OnTriggerEnter(Collider collider)
    //   {
    //       if (collider.tag == "Player")
    //       {
    //           if (complete) return;
    //           if (!running)
    //           {
    //               ResetAll();
    //               running = true;
    //           }
    //           currentTime = Time.time;
    //       }
    //   }

}