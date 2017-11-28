using UnityEngine;
using System.Collections.Generic;
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

    public scoreManager m_scoreManager;
    public Loading m_loading;

    void Awake () {
        EventManager<GameEvent>.Add(HandleMessage);

        Debug.Log(Application.persistentDataPath);

    }
    void Start () {

        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent._NULL_);
        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(UIManager), GameEvent.STATE_MENU);
        Time.timeScale = 0;

        GetComponent<AudioManager>().PlayMusic(0, true);

        SavingSystem.Load();
    }

    public void HandleMessage(object s, __eArg<GameEvent> e) {
        if (s == (object)this) return;
        switch (e.arg) {
        case GameEvent.STATE_LOSE_SCREEN:
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            Time.timeScale = 0;
            break;
        case GameEvent.STATE_WIN_SCREEN:
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;

            SavingSystem.Add(":NAME:", m_scoreManager.getFinalScore());
            SavingSystem.Save();

            break;
        case GameEvent.STATE_MENU:
        case GameEvent.STATE_PAUSE:
            GetComponent<AudioManager>().FadeOutMusic(0, 1);
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
            if (e.type != GetType()) break;
            Time.timeScale = 1;
            if (m_loading.IsComplete())
            {
                GetComponent<AudioManager>().FadeInMusic(0, 5);
                GameObject.FindObjectOfType<Player>().enabled = true;
                GameObject.FindObjectOfType<PlayerCamera>().enabled = true;
                GameObject.FindObjectOfType<PlayerMovement>().enabled = true;
                GameObject.FindObjectOfType<CombinedScript>().enabled = true;

                GetComponent<AudioManager>().FadeInMusic(0, 1);

                m_loading.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                
                EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            }
            else
            {
                GetComponent<AudioManager>().PlayMusic(0, true);
                m_loading.Begin();
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

