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

    public Loading m_loading;
    public UnityEngine.UI.Text m_highScores;

    void Awake () {
        EventManager<GameEvent>.Add(HandleMessage);
        
        Debug.Log(Application.persistentDataPath);

    }
    void Start () {

        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent._NULL_);
        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(UIManager), GameEvent.STATE_MENU);
        Time.timeScale = 0;

        SavingSystem.Load();
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.L)) {
            EventManager<GameEvent>.InvokeGameState(null, null, null, typeof(GameManager), GameEvent.STATE_HIGHSCORE);
        }
    }

    public void HandleMessage(object s, __eArg<GameEvent> e) {
        if (s == (object)this) return;
        switch (e.arg) {
        case GameEvent.STATE_HIGHSCORE:
            break;
        case GameEvent.STATE_LEADERBOARD:
            m_highScores.text = "High Scores:\n";
            for (int i = 0; i < SavingSystem.m_data.name.Count; i++) {
                m_highScores.text += SavingSystem.m_data.name[i] + "\t\t" + SavingSystem.m_data.score[i] + "\n";
            }
            break;
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


            break;
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
            if (e.type != GetType()) break;
            Time.timeScale = 1;
            if (m_loading.IsComplete())
            {
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
                m_loading.Begin();
                GetComponent<AudioManager>().StopMusic(0);
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

