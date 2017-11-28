using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SavingSystem {
    public static SavingData m_data;

    public static void Add(string name, int score) {
        if (m_data == null) m_data = new SavingData();
        if (m_data.name.Count == 0) {
            m_data.name.Add(name);
            m_data.score.Add(score);
        } else {
            for (int i = 0; i < m_data.name.Count; i++) {
                if (i > 5) return;
                if (score > m_data.score[i]) {
                    m_data.name.Insert(i, name);
                    m_data.score.Insert(i, score);
                    break;
                }
            }
            if (m_data.name.Count > 5) {
                m_data.name.RemoveRange(6, m_data.name.Count);
            }
        }
    }
    // saves all the data
    public static void Save () {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Leaderboard.dat");

        SavingData data = new SavingData();
        data.name = m_data.name;
        data.score = m_data.score;

        bf.Serialize(file, data);
        file.Close();
    }
    // Loads in all the data
    public static void Load () {
        if (m_data == null)
            m_data = new SavingData();

        if (File.Exists(Application.persistentDataPath + "/Leaderboard.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Leaderboard.dat", FileMode.Open);
            SavingData data = (SavingData)bf.Deserialize(file);
            file.Close();

            m_data.name = data.name;
            m_data.score = data.score;
        }
    }
}
[System.Serializable]
public class SavingData {
    public SavingData () {
        name = new List<string>();
        score = new List<int>();
    }
    public List<string> name;
    public List<int> score;
}