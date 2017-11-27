using UnityEngine;
using UnityEngine.SceneManagement;

// Struct holding all the Game info
[System.Serializable] struct GameInfo {
    public int m_lightIntensity;
    public Color m_LightColor;
    public Color m_UIColor;
}

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(EnemyManager))]
public class GameManager : MonoBehaviour {
    [SerializeField] GameInfo m_gameInfo;

    public Loading m_loading;

    void Awake () {
        EventManager<GameEvent>.Add(HandleMessage);
    }
    void Start () {
        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent._NULL_);
        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(UIManager), GameEvent.STATE_MENU);
    }

    public void HandleMessage(object s, __eArg<GameEvent> e) {
        if (s == (object)this) return;
        switch (e.arg) {
        case GameEvent.STATE_MENU:
        case GameEvent.STATE_PAUSE:
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;

            if (e.type == GetType())
                EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            break;
        case GameEvent.STATE_RESTART:
                __event<GameEvent>.UnsubscribeAll();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
        case GameEvent.STATE_START:
        case GameEvent.STATE_CONTINUE:
            Time.timeScale = 1;
            if (e.type != GetType()) break;
            if (m_loading.IsComplete())
            {
                GetComponent<AudioManager>().FadeIn(GetComponent<AudioManager>().musics[0], 1);
                m_loading.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                
                EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            }
            else
            {
                m_loading.Begin();
                GetComponent<AudioManager>().Stop(GetComponent<AudioManager>().musics[0]);
            }
            break;
        case GameEvent.PICKUP_RIFLEAMMO:
        case GameEvent.PICKUP_SHOTGUNAMMO:
        case GameEvent.PICKUP_HEALTH:
            if (e.type == GetType())
                EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            break;
        case GameEvent.DIFFICULTY_EASY:
        case GameEvent.DIFFICULTY_MEDI:
        case GameEvent.DIFFICULTY_HARD:
        case GameEvent.DIFFICULTY_NM:
            EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            break;
        }
    }
}
