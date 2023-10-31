using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreFeedBack : MonoBehaviour
{
    GameObject scoreFeedBackGO;
    Text scoreFeedBackTX;
    float textAlpha = 0, activeTime = 1.0f, scoreDisplay, diffGoal;
    const float resetActiveTime = 1.0f;
    public static float scoreDiff, diffBeforeMulti;
    public static Vector2 feedBackPos;
    bool isWorking = false;
    Color textColor;
    // Start is called before the first frame update
    void Start()
    {
        scoreFeedBackGO = this.gameObject;
        scoreFeedBackTX = GetComponent<Text>();
        textColor = scoreFeedBackTX.color;
        scoreDiff = 0;
        diffBeforeMulti = 0;
        diffGoal = 0;
        textAlpha = 0;
        activeTime  = 1.0f;
     
        isWorking = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (scoreDiff != 0)
        {
            if (BuffManagement.buffTrigger[0]) 
            {
                scoreDisplay = diffBeforeMulti;
                diffGoal = scoreDiff;
            }
            else 
            {
                scoreDisplay = scoreDiff;
                scoreFeedBackTX.text = String.Format("{0:####}", scoreDisplay);
                if (scoreDiff < 0)
                {
                    scoreFeedBackTX.text = "-" + String.Format("{0:####}", scoreDisplay);
                }
            }
     
          
            scoreFeedBackGO.transform.position = feedBackPos;
            textAlpha = 1;
            isWorking = true;
            scoreDiff = 0;
            diffBeforeMulti = 0;
            activeTime = resetActiveTime    ;
        }

        if (isWorking) 
        {
            textAlpha -= Time.deltaTime;
            activeTime -= Time.deltaTime;
            gainScoreDisplay();
        }

        if (activeTime < 0)
        {
            isWorking=false;
        }
        textColor.a = textAlpha;
        scoreFeedBackTX.color = textColor;
    }

    IEnumerator gainScoreDisplay() 
    {
        while (scoreDisplay < diffGoal)
        {
            scoreDisplay += diffGoal   / 50;
            scoreFeedBackTX.text = String.Format("{0:####}", scoreDisplay);
            if (diffGoal < 0)
            {
                scoreFeedBackTX.text = "-" + String.Format("{0:####}", scoreDisplay);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
