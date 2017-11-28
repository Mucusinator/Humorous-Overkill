using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour {
    public scoreManager m_scoreManager;

    public void AddHighScore () {
        SavingSystem.Add(":NAME:", m_scoreManager.getFinalScore());
        SavingSystem.Save();

        GetComponent<UIAction>().SendEvent();
    }
}
