using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapEventHandler))]
public class Map : MonoBehaviour {

    // Game Checks
    private bool isEnabled = true;
    private bool isInitialized = false;
    private bool isFullscreen = false;

    // Variables
    Transform m_player;
    RectTransform m_map;

    void Awake () {
        // Raise and Invoke Event
        __event<MapState>.HandleEvent += new __eHandle<System.Object, __eArg<MapState>>(OnHandleEvent);

        // Ping its location to all others that are listening
        __event<MapState>.InvokeEvent(
            this,
            new __eArg<MapState>(
                MapState.PING,
                this,
                this,
                GetType() ));
    }

    void Init () {
        // Init Variables
        m_map = this.GetComponent<RectTransform>();

        
        // Initialization Finished
        isInitialized = true;
    }

    void Update () {
        if (!isInitialized) return;

        Vector3 r = m_map.eulerAngles;
        r.z = m_player.eulerAngles.y;
        m_map.eulerAngles = r;

    }

    public void OnHandleEvent (object s, __eArg<MapState> e) {
        // If not enabled, don't check for events
        if (!isEnabled) return;

        // Log Events that happen after Event has been Raised
        if (e.target != (System.Object)this && s != (System.Object)this || e.target == __event<MapState>.SendToAll) {
            // Logging
            //_Debug.Log(":: Args (" + e.arg.ToString() + ") To <" + e.target + ">\nfrom <" + s + "> This <" + this + ">");

            // Get player ping
            if (e.arg == MapState.PING && e.type == typeof(Player)) { m_player = (Transform)e.value; }

            // Check if map is enabled
            if (e.arg == MapState.NOTENABLED) { isEnabled = false; }

            // Check if map has been initialized
            if (e.arg == MapState.INIT && isEnabled) { Init(); }

            // Check if map is fullscreen
            if (e.arg == MapState.FULLSCREENMAP) {
                isFullscreen = true;
                m_map.position = new Vector2(Screen.width / 2, Screen.height / 2);
                m_map.sizeDelta = new Vector2(300, 300);
            }

            // Check if map is minimized
            if (e.arg == MapState.MINIMAP) {
                isFullscreen = false;
                m_map.position = new Vector2(Screen.width - 200, 150);
                m_map.sizeDelta = new Vector2(100, 100);
            }
        }
    }
}
