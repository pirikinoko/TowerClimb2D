﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{

    public Text timer;
    public Text countDown;
    public GameObject loadAnimGO;
    public static float elapsedTime , playTime, pastTime, startTime = 3f; //操作用, 表示用, 総経過時間, カウントダウン
    public float setTime;
    float soundTime = 1f;
    bool startFlag;
    SoundEffect soundEffect;
    // Start is called before the first frame update
    void Start()
    {
        //loadAnimGO.SetActive(true);
        playTime = setTime;
        elapsedTime = setTime;
        pastTime = 0;
        soundTime = -1;
        startTime = 3f;
        startFlag = true;
        countDown.text = ("3");
        soundEffect = GameObject.Find("AudioSystem").GetComponent<SoundEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        TimerStart();
        if(pastTime > 10)
        {
            loadAnimGO.SetActive(false);
        }
    }

    void TimerStart()
    {
        //カウントダウン
        if (startFlag)
        {
            startTime -= Time.deltaTime;
            soundTime -= Time.deltaTime;
            if (startTime > 2)
            {
                countDown.text = ("3");
            }
            else if (startTime > 1)
            {
                countDown.text = ("2");
            }
            else if (startTime > 0)
            {
                countDown.text = ("1");
            }
            else if (startTime < 0 && startTime > -0.5f)
            {
                countDown.text = ("スタート");
                GameSystem.playable = true;
            }
            else if (startTime < -0.5f)
            {
                countDown.text = ("");
                startFlag = false;
            }
            if (soundTime < 0)
            {
                soundEffect.PlaySE(1);
                soundTime = 1;
            }
        }
        //ゲームスタート
        if (startTime < 0)
        {      
            if (GameSystem.playable)
            {
                pastTime += Time.deltaTime;
                elapsedTime -= Time.deltaTime;
                playTime = elapsedTime * 10;
                playTime = Mathf.Floor(playTime) / 10;
                timer.text = ("タイム:" + playTime);
            }

        }
    }
}

