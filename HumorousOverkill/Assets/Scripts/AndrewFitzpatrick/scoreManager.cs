using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// require text component
[RequireComponent(typeof(Text))]
public class scoreManager : MonoBehaviour
{
    // difficulty enum
    public enum DIFFICULTY { EASY, NORMAL, HARD, NIGHTMARE };

    // types of enemy
    public enum ENEMYTYPE { DONUT, CUPCAKE };

    // contains point values for each difficulty
    [System.Serializable]
    public struct pointValues
    {
        public int easyPoints;
        public int normalPoints;
        public int hardPoints;
        public int nightmarePoints;
    }

    // contains multipliers for different completion times
    [System.Serializable]
    public struct timeMultiplier
    {
        public float completionTime;
        public float scoreMultiplier;
    }

    // this will be set by someone else
    public DIFFICULTY difficulty = DIFFICULTY.NORMAL;

    // point values for donuts
    public pointValues donutScoreValues;

    // point values for cupcakes
    public pointValues cupcakeScoreValues;

    // current score
    public int currentScore = 0;

    [Tooltip("Will be displayed in front of the current score")]
    public string displayString;

    [Tooltip("Will be displayed in front of the final score")]
    public string finalDisplayString;

    public List<timeMultiplier> timeMultipliers = new List<timeMultiplier>();

    // total number of enemies in the game
    private int totalGameEnemies;

    // refernce to Text
    private Text myText;

    // reference to timer
    private timer myTimer;

    public void Awake()
    {
        // get Text
        myText = GetComponent<Text>();

        // set Text
        myText.text = displayString + currentScore;

        // find timer (if it exists)
        if(GameObject.FindObjectsOfType<timer>().Length != 0)
        {
            myTimer = GameObject.FindObjectsOfType<timer>()[0];
        }
    }

    public bool updatePoints(ENEMYTYPE e)
    {
        switch(e)
        {
            case (ENEMYTYPE.DONUT):
                // add correct ammount of points
                currentScore += getPoints(e);

                // update score text
                myText.text = displayString + currentScore;
                break;
        }

        return true;
    }

    // returns the ammount of points for an enemy on the current difficulty
    int getPoints(ENEMYTYPE e)
    {
        // donut
        if(e == ENEMYTYPE.DONUT)
        {
            switch (difficulty)
            {
                case DIFFICULTY.EASY:
                    return donutScoreValues.easyPoints;
                case DIFFICULTY.NORMAL:
                    return donutScoreValues.normalPoints;
                case DIFFICULTY.HARD:
                    return donutScoreValues.hardPoints;
                case DIFFICULTY.NIGHTMARE:
                    return donutScoreValues.nightmarePoints;
            }
        }
        // cupcake
        else if(e == ENEMYTYPE.CUPCAKE)
        {
            switch (difficulty)
            {
                case DIFFICULTY.EASY:
                    return cupcakeScoreValues.easyPoints;
                case DIFFICULTY.NORMAL:
                    return cupcakeScoreValues.normalPoints;
                case DIFFICULTY.HARD:
                    return cupcakeScoreValues.hardPoints;
                case DIFFICULTY.NIGHTMARE:
                    return cupcakeScoreValues.nightmarePoints;
            }
        }

        return 0;
    }

    public int getFinalScore()
    {
        float finalScore = currentScore;
        float finalTime = myTimer.elapsedTime;

        // multiply the score by the correct ultiplier based on completion time
        foreach(timeMultiplier currentMultiplier in timeMultipliers)
        {
            if(finalTime < currentMultiplier.completionTime)
            {
                finalScore *= currentMultiplier.scoreMultiplier;
                break;
            }
        }

        // round and return
        return Mathf.RoundToInt(finalScore);
    }
}
