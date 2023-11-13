using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreFeedBack : MonoBehaviour
{
    public Transform parentObject; // 親となるゲームオブジェクト
    GameObject[] scoreFeedBackGO = new GameObject[10], buffMultiTextGO = new GameObject[10];
    Text[] scoreFeedBackTX = new Text[10], buffMultiTx = new Text[10];
    Color[] textColor = new Color[10];
    float[] textAlpha = new float[10], activeTime = new float[10];
    bool[] isWorking = new bool[10];
    int[] coloutineCount = new int[10];

    int SFBNum, BMTNum;

    float  scoreDisplay, diffGoal;
    const float resetActiveTime = 1.0f;
    public static float scoreDiff, diffBeforeMulti;
    public static Vector2 feedBackPos;

    // Start is called before the first frame update
    void Start()
    {
       
        scoreDiff = 0;
        diffBeforeMulti = 0;
        diffGoal = 0;

        SFBNum = 0;
        BMTNum = 0;

        for (int i = 0; i < scoreFeedBackGO.Length; i++)
        {
            activeTime[i] = 1.0f;
            textAlpha[i]= 0;
            isWorking [i]= false;
            coloutineCount[i]= 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        BuffManagement.buffTrigger[0] = true;
        if (scoreDiff != 0)
        {
            GameObject scoreFeedBackPrefab = (GameObject)Resources.Load("feedBackText");
            scoreFeedBackGO[SFBNum]  = Instantiate(scoreFeedBackPrefab, feedBackPos, Quaternion.identity, parentObject);
            scoreFeedBackTX[SFBNum] = scoreFeedBackGO[SFBNum].GetComponent<Text>();
            textColor[SFBNum] = scoreFeedBackTX[SFBNum].color;
            if (BuffManagement.buffTrigger[0])
            {
                scoreDisplay = diffBeforeMulti;
                diffGoal = scoreDiff;
            }
            else
            {
                scoreDisplay = scoreDiff;
                scoreFeedBackTX[SFBNum].text = String.Format("{0:####}", scoreDisplay);
                if (scoreDiff < 0)
                {
                    scoreFeedBackTX[SFBNum].text = "-" + String.Format("{0:####}", scoreDisplay);
                }
            }

            textAlpha[SFBNum] = 1;
            isWorking[SFBNum] = true;
            coloutineCount[SFBNum] = 0;
            scoreDiff = 0;
            diffBeforeMulti = 0;



            SFBNum++;
            if (SFBNum == 10) { SFBNum = 0; }
        }

        for (int i = 0; i < scoreFeedBackGO.Length; i++)
        {
            if (isWorking[i])
            {
                textAlpha[i] -= 0.6f * Time.deltaTime;
                activeTime[i] -= 0.6f * Time.deltaTime;
                if (scoreDisplay < diffGoal && BuffManagement.buffTrigger[0] && coloutineCount[i] == 0)
                {
                    StartCoroutine(gainScoreDisplay(i));
                    coloutineCount[i] = 1;
                }
                textColor[i].a = textAlpha[i];
                scoreFeedBackTX[i].color = textColor[i];

                if (activeTime[i] < 0)
                {
                    isWorking[i] = false;
                    Destroy(scoreFeedBackGO[i]);
                    activeTime[i] = resetActiveTime;
                }
            }

      

        }
     
    }

    IEnumerator gainScoreDisplay(int targetSFB) 
    {
        Vector2 buffMultiPos = scoreFeedBackGO[targetSFB].transform.position;
        float buffMulti = 3.0f;
        buffMultiPos.x += 0.2f;
        buffMultiPos.y += 0.2f;
        GameObject buffMultiPrefab = (GameObject)Resources.Load("BuffMultiText");
        buffMultiTextGO[targetSFB] = Instantiate(buffMultiPrefab, buffMultiPos, Quaternion.identity, parentObject);
        buffMultiTx[targetSFB] = buffMultiTextGO[targetSFB].GetComponent<Text>();

        scoreFeedBackTX[targetSFB].text = String.Format("{0:####}", scoreDisplay);
        buffMultiTx[targetSFB].text = String.Format("x" + "{0:####}", buffMulti);
        if (diffGoal < 0)
        {
            scoreFeedBackTX[targetSFB].text = "-" + String.Format("{0:####}", scoreDisplay);
            buffMultiTx[targetSFB].text = String.Format("x" + "{0:####}", buffMulti);
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(buffMultiTextGO[targetSFB]);


        while (scoreDisplay < diffGoal)
        {
            scoreDisplay += diffGoal   / 200;
            scoreFeedBackTX[targetSFB].text = String.Format("{0:####}", scoreDisplay);
            buffMultiTx[targetSFB].text = String.Format("x" +  buffMulti);
            if (diffGoal < 0)
            {
                scoreFeedBackTX[targetSFB].text = "-" + String.Format("{0:####}", scoreDisplay);
                buffMultiTx[targetSFB].text = String.Format("x" + "{0:####}", buffMulti);
            }
            yield return new WaitForSeconds(0.05f * Time.deltaTime);
        }
    }
}
