using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public scoreManager m_scoreManager;
    public UnityEngine.UI.Text m_text;
    public void AddHighScore()
    {
        if (m_text.text == "") return;
        SavingSystem.Add(m_text.text, m_scoreManager.getFinalScore());
        SavingSystem.Save();

        GetComponent<UIAction>().SendEvent();
    }
}
