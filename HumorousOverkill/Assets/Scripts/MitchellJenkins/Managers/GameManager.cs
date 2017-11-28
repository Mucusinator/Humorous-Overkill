using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

    public SavingData m_data;
    public Loading m_loading;

    void Awake () {
        EventManager<GameEvent>.Add(HandleMessage);
        
        Debug.Log(Application.persistentDataPath);

    }
    void Start () {
        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent._NULL_);
        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(UIManager), GameEvent.STATE_MENU);
        Time.timeScale = 0;
    }

    public void Save () {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Leaderboard.dat");

        SavingData data = new SavingData();
        data.name = m_data.name;
        data.score = m_data.score;

        bf.Serialize(file, data);
        file.Close();
    }
    // Loads in all the data
    public void Load () {
        if (File.Exists(Application.persistentDataPath + "/Leaderboard.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Leaderboard.dat", FileMode.Open);
            SavingData data = (SavingData)bf.Deserialize(file);
            file.Close();

            m_data.name = data.name;
            m_data.score = data.score;
        }
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

            m_data = new SavingData();
            m_data.score = new List<int>();
            m_data.score.Add(50);
            Save();

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

[System.Serializable]
public struct SavingData {
    public List<string> name;
    public List<int> score;
}