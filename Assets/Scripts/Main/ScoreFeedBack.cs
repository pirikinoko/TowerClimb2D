using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreFeedBack : MonoBehaviour
{
    GameObject scoreFeedBackGO, buffMultiTextGO; 
    Text scoreFeedBackTX, buffMultiTx;
    float textAlpha = 0, activeTime = 1.0f, scoreDisplay, diffGoal;
    const float resetActiveTime = 1.0f;
    public static float scoreDiff, diffBeforeMulti;
    public static Vector2 feedBackPos;
    bool isWorking = false, stopCoroutine = false;
    Color textColor;
    // Start is called before the first frame update
    void Start()
    {
        scoreFeedBackGO = this.gameObject;
        scoreFeedBackTX = GetComponent<Text>();
        buffMultiTextGO = GameObject.Find("BuffMultiText");
        buffMultiTx = buffMultiTextGO.GetComponent<Text>();
        textColor = scoreFeedBackTX.color;
        scoreDiff = 0;
        diffBeforeMulti = 0;
        diffGoal = 0;
        textAlpha = 0;
        activeTime  = 1.0f;
        stopCoroutine = true;
        isWorking = false;
    }

    // Update is called once per frame
    void Update()
    {
        BuffManagement.buffTrigger[0] = true;
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
            activeTime = resetActiveTime;
            stopCoroutine = true;
        }

        if (isWorking) 
        {
            textAlpha -= 0.6f * Time.deltaTime;
            activeTime -= 0.6f * Time.deltaTime;
            if (scoreDisplay < diffGoal && BuffManagement.buffTrigger[0])
            { 
                StartCoroutine(gainScoreDisplay());
            }
             
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
        Vector2 buffMultiPos = scoreFeedBackGO.transform.position;
        float buffMulti = 3.0f;
        buffMultiPos.x += 0.2f;
        buffMultiPos.y += 0.2f;
        buffMultiTextGO.gameObject.SetActive(true);
        buffMultiTextGO.transform.position = buffMultiPos;

        scoreFeedBackTX.text = String.Format("{0:####}", scoreDisplay);
        buffMultiTx.text = String.Format("x" + "{0:####}", buffMulti);
        if (diffGoal < 0)
        {
            scoreFeedBackTX.text = "-" + String.Format("{0:####}", scoreDisplay);
            buffMultiTx.text = String.Format("x" + "{0:####}", buffMulti);
        }

        
        for (int i = 0; i < 50; i++)
        {      
            if (stopCoroutine) { Debug.Log("Working"); buffMultiTextGO.gameObject.SetActive(false); stopCoroutine = false; yield break; }
            yield return new WaitForSeconds(0.008f);
        }
      

        while (scoreDisplay < diffGoal)
        {
            if (stopCoroutine) { buffMultiTextGO.gameObject.SetActive(false); stopCoroutine = false; yield break; }
            scoreDisplay += diffGoal   / 200;
            scoreFeedBackTX.text = String.Format("{0:####}", scoreDisplay);
            buffMultiTx.text = String.Format("x" +  buffMulti);
            if (diffGoal < 0)
            {
                scoreFeedBackTX.text = "-" + String.Format("{0:####}", scoreDisplay);
                buffMultiTx.text = String.Format("x" + "{0:####}", buffMulti);
            }
            yield return new WaitForSeconds(0.05f * Time.deltaTime);

        }
        buffMultiTextGO.gameObject.SetActive(false);
    }
}
