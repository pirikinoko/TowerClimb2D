using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{

    public Text timer;
    public Text countDown;
    public static float elapsedTime;
    public static float playTime = 15;
    public static float startTime = 3f;
    public float TimeSet;
    float soundTime = 1f;
    bool startFlag;

    // Start is called before the first frame update
    void Start()
    {
        playTime = TimeSet;
        elapsedTime = TimeSet;
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
                elapsedTime -= Time.deltaTime;
                playTime = elapsedTime * 10;
                playTime = Mathf.Floor(playTime) / 10;
                timer.text = ("タイム:" + playTime);
            }

        }
    }
}

