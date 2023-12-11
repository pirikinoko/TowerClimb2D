using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreFeedBack : MonoBehaviour
{
    public Transform parentObject; // �e�ƂȂ�Q�[���I�u�W�F�N�g
    GameObject[] scoreFeedBackGO = new GameObject[10], buffMultiTextGO = new GameObject[10];
    Text[] scoreFeedBackTX = new Text[10], buffMultiTx = new Text[10];
    Color[] textColor = new Color[10];
    float[] textAlpha = new float[10], activeTime = new float[10], spownTime = new float[10];
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
        if (scoreDiff != 0)
        {
            GameObject scoreFeedBackPrefab = (GameObject)Resources.Load("feedBackText");
            scoreFeedBackGO[SFBNum]  = Instantiate(scoreFeedBackPrefab, feedBackPos, Quaternion.identity, parentObject);
            scoreFeedBackTX[SFBNum] = scoreFeedBackGO[SFBNum].GetComponent<Text>();
            scoreFeedBackGO[SFBNum].name = "FeedBackText" + SFBNum;
            spownTime[SFBNum] = TimeScript.pastTime;
            if (BuffManagement.buffTrigger[0] && spownTime[SFBNum] > BuffManagement.buffStart[0])
            {
                scoreFeedBackGO[SFBNum].AddComponent<GainScoresAnime>();
                scoreFeedBackGO[SFBNum].GetComponent<GainScoresAnime>().scoreDisplay = diffBeforeMulti;
                scoreFeedBackGO[SFBNum].GetComponent<GainScoresAnime>().diffGoal = scoreDiff;
                Vector2 buffMultiPos = scoreFeedBackGO[SFBNum].transform.position;
                buffMultiPos.x += 0.2f;
                buffMultiPos.y += 0.2f;
                GameObject buffMultiPrefab = (GameObject)Resources.Load("BuffMultiText");
                buffMultiTextGO[SFBNum] = Instantiate(buffMultiPrefab, buffMultiPos, Quaternion.identity, parentObject);
                buffMultiTextGO[SFBNum].name = "BuffMultiText" + SFBNum;
            }
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
            scoreDiff = 0;
            diffBeforeMulti = 0;



            SFBNum++;
            if (SFBNum == 10) { SFBNum = 0; }
        }

        for (int i = 0; i < scoreFeedBackGO.Length; i++)
        {
            if (isWorking[i])
            {
                textAlpha[i] -= 0.4f * Time.deltaTime;
                activeTime[i] -= 0.6f * Time.deltaTime;


                if (activeTime[i] < 0)
                {
                    isWorking[i] = false;
                    activeTime[i] = resetActiveTime;
                    if (!BuffManagement.buffTrigger[0])
                    {
                        Destroy(scoreFeedBackGO[i]);

                    }
                }
                if (scoreFeedBackGO[i] == null) { return; }
                textColor[i].a = textAlpha[i];
                scoreFeedBackTX[i].color = textColor[i];
            }
        }
    }
}
