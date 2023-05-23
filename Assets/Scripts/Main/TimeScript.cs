using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{

    public Text timer;
    public Text countDown;
    public static float elapsedTime , playTime, pastTime; //操作用, 表示用, 総経過時間
    public float setTime;
    float startTime = 3f,soundTime = 1f;
    bool startFlag;

    // Start is called before the first frame update
    void Start()
    {
        playTime = setTime;
        elapsedTime = setTime;
        pastTime = 0;
        soundTime = 1f;
        startTime = 3f;
        startFlag = true;
        countDown.text = ("3");
        SoundEffect.BunTrigger = true;

    }

    // Update is called once per frame
    void Update()
    {
        TimerStart();
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
                SoundEffect.BunTrigger = true;
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

