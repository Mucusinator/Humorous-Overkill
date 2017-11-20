using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// require text component
[RequireComponent(typeof(Text))]
[EventHandler.BindListener("timer", typeof(timer))]
public class scoreManager : EventHandler.EventHandle
{
    // difficulty enum
    public enum DIFFICULTY { EASY, NORMAL, HARD };

    // contains point values for each difficulty
    [System.Serializable]
    public struct pointValues
    {
        public int easyPoints;
        public int normalPoints;
        public int hardPoints;
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

    public override void Awake()
    {
        // do base Awake
        base.Awake();

        // get total game enemies from enemyManager

        // get Text
        myText = GetComponent<Text>();

        // set Text
        myText.text = displayString + currentScore;
    }

    public override bool HandleEvent(GameEvent e, float value)
    {
        switch(e)
        {
            case (GameEvent.ENEMY_DIED):
                // add correct ammount of points
                currentScore += getPoints(value);

                // update score text
                myText.text = displayString + currentScore;
                break;
        }

        return true;
    }

    // returns the ammount of points for an enemy on the current difficulty
    int getPoints(float enemyType)
    {
        // donut
        if(enemyType == 0)
        {
            if(difficulty == DIFFICULTY.EASY)
            {
                return donutScoreValues.easyPoints;
            }
            if (difficulty == DIFFICULTY.NORMAL)
            {
                return donutScoreValues.normalPoints;
            }
            if (difficulty == DIFFICULTY.HARD)
            {
                return donutScoreValues.hardPoints;
            }
        }
        // cupcake
        else if(enemyType == 1)
        {
            if (difficulty == DIFFICULTY.EASY)
            {
                return cupcakeScoreValues.easyPoints;
            }
            if (difficulty == DIFFICULTY.NORMAL)
            {
                return cupcakeScoreValues.normalPoints;
            }
            if (difficulty == DIFFICULTY.HARD)
            {
                return cupcakeScoreValues.hardPoints;
            }
        }

        return 0;
    }

    public int getFinalScore()
    {
        float finalScore = currentScore;
        float finalTime = GetEventListener("timer").GetComponent<timer>().elapsedTime;

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
