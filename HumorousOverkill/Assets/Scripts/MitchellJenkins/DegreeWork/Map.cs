using UnityEngine;

[RequireComponent(typeof(MapEventHandler))]
public class Map : MonoBehaviour {

    // Game Checks
    private bool isVisable = false;
    private bool isEnabled = true;
    private bool isInitialized = false;
    private bool isFullscreen = false;


    // Tracking Positions
    public Transform m_player;
    public Transform[] m_enemy;
    // Map texture
    public Texture2D m_map;
    // Map Icons
    public Texture2D m_playerIcon;
    public Texture2D m_enemyIcon;
    // Map Variables
    public float m_mapOffSetX;
    public float m_mapOffSetY;
    public float m_mapWidth;
    public float m_mapHeight;
    public float m_sceneWidth;
    public float m_sceneHeight;
    public float m_iconSize;
    public float m_sceneOffSet;
    float pX;
    float pZ;
    float playerMapX;
    float playerMapZ;

    void Awake () {
        // Raise and Invoke Event
        __event<MapState>.HandleEvent += new __eHandle<System.Object, __eArg<MapState>>(OnMapHandleEvent);
        __event<GameEvent>.HandleEvent += new __eHandle<System.Object, __eArg<GameEvent>>(OnGUIHandleEvent);

        // Ping its location to all others that are listening
        __event<MapState>.InvokeEvent(
            this,
            new __eArg<MapState>(
                MapState.PING,
                this,
                this,
                GetType() ));
    }

    float MapPos(float pos, float mapSize, float sceneSize) {
        return pos * mapSize / sceneSize;
    }

    void Init () {
        // Init Variables

        
        // Initialization Finished
        isInitialized = true;
    }

    void OnGUI () {
        if (!isInitialized) return;
        if (!isVisable) return;
        GUI.BeginGroup(new Rect(m_mapOffSetX, m_mapOffSetY, m_mapWidth, m_mapHeight), m_map);
        pX = MapPos(transform.position.x, m_mapWidth, m_sceneWidth);
        pZ = MapPos(transform.position.z, m_mapHeight, m_sceneHeight);
        playerMapX = pX - (m_iconSize/2);
        playerMapZ = ((pZ * -1) - (m_iconSize / 2)) + m_mapHeight;
        GUI.Box(new Rect(playerMapX * m_sceneOffSet, playerMapZ * m_sceneOffSet, m_iconSize, m_iconSize), m_playerIcon, new GUIStyle());
        GUI.EndGroup();
    }

    public void OnGUIHandleEvent (object s, __eArg<GameEvent> e) {
        if (e.type != null) return;
        switch (e.arg) {
        case GameEvent.STATE_START:
        case GameEvent.STATE_CONTINUE:
            isVisable = true;
            break;
        case GameEvent.STATE_MENU:
        case GameEvent.STATE_PAUSE:
        case GameEvent.STATE_RESTART:
        case GameEvent.STATE_WIN_SCREEN:
        case GameEvent.STATE_LOSE_SCREEN:
        case GameEvent.STATE_DIFFICULTY:
            isVisable = false;
            break;
        }
    }

    public void OnMapHandleEvent (object s, __eArg<MapState> e) {
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
            }

            // Check if map is minimized
            if (e.arg == MapState.MINIMAP) {
                isFullscreen = false;
            }
        }
    }
}
