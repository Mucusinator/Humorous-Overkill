using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// require text component
[RequireComponent(typeof(Text))]
public class timer : MonoBehaviour
{
    private float elapsedTime = 0;
    private bool isTiming = true;
    private Text myText;

    // allow designers to modify text
    [Tooltip("Text to be displayed. use MIN for minutes, SEC for seconds, and MIL for milliseconds")]
    public string displayTextFormat;

    // runs once when this timer is created at the start of the main scene
    void Start()
    {
        // don't destroy the timer when loading other scenes
        DontDestroyOnLoad(this.gameObject);

        // get text component
        myText = GetComponent<Text>();

        // elapsed time is currently 0
        elapsedTime = 0.0f;
    }

    void Update()
    {
        // add to elapsed game
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
        }
        //Debug.Log(elapsedTime);
    }

    // update the text each frame to display the time
    void OnGUI()
    {
        float minutes = elapsedTime / 60.0f;
        float seconds = elapsedTime;
        float milliseconds = elapsedTime * 1000.0f;
        string displayString = displayTextFormat;
        if(displayString.Contains("MIN"))
        {
            displayString.Replace("MIN", minutes.ToString());
            Debug.Log("MIN");
        }
        if (displayString.Contains("SEC"))
        {
            displayString.Replace("SEC", seconds.ToString());
            Debug.Log("SEC");
        }
        if (displayString.Contains("MIL"))
        {
            displayString.Replace("MIL", milliseconds.ToString());
            Debug.Log("MIL");
        }
        Debug.Log(displayString);
        myText.text = seconds.ToString();
    }

    // allow future scripts to toggle whether the timer should be timing
    // pause menu, etc.
    public void toggleTiming()
    {
        isTiming = !isTiming;
    }

    // returns the elapsed time
    public float getElapsedTime()
    {
        return elapsedTime;
    }
}
