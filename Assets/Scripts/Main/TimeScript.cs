using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{
    public GameObject timeGO, canvasObj;
    public Text timer;
    public Text countDown;
    public Text startText;
    public GameObject loadAnimGO;
    public static float elapsedTime , playTime, pastTime, startTime = 3f; //操作用, 表示用, 総経過時間, カウントダウン
    public float setTime, timeLastFrame;
    float soundTime = 1f, changeSpeed = 1, minOpacity = 0, maxOpacity = 1;
    bool startFlag, increasingOpacity;
    bool sizeEffectCoroutine;
    SoundEffect soundEffect;
    // Start is called before the first frame update
    void Start()
    {
        //loadAnimGO.SetActive(true);
        playTime = setTime;
        timeLastFrame = setTime;
        elapsedTime = setTime;
        pastTime = 0;
        soundTime = -1;
        startTime = 3f;
        startFlag = false;
        sizeEffectCoroutine = false;
        countDown.text = ("");
        soundEffect = GameObject.Find("AudioSystem").GetComponent<SoundEffect>();
        startText.text = "スペースを押してスタート";
    }

    // Update is called once per frame
    void Update()
    {
     

        if (Input.GetKeyDown(KeyCode.Space))
        {
            startFlag = true;
        }
        TimerStart();
        if(pastTime > 10)
        {
            loadAnimGO.SetActive(false);
        }

        //スタート前表示
        if (startFlag == false)
        {
            Color currentColor = startText.color;
            if (increasingOpacity)
            {
                currentColor.a += changeSpeed * Time.deltaTime;
                if (currentColor.a >= maxOpacity)
                {
                    currentColor.a = maxOpacity;
                    increasingOpacity = false;
                }
            }
            else
            {
                currentColor.a -= changeSpeed * Time.deltaTime;
                if (currentColor.a <= minOpacity)
                {
                    currentColor.a = minOpacity;
                    increasingOpacity = true;
                }
            }

            // 変更した透明度を適用
            startText.color = currentColor;
            Debug.Log(startText.color);
        }
        else
        {
            startText.text = "";
        }

    }

    //private void FixedUpdate()
    //{
    //    if (startFlag == false)
    //    {
    //        Color currentColor = startText.color;
    //        if (increasingOpacity)
    //        {
    //            currentColor.a += changeSpeed * Time.deltaTime;
    //            if (currentColor.a >= maxOpacity)
    //            {
    //                currentColor.a = maxOpacity;
    //                increasingOpacity = false;
    //            }
    //        }
    //        else
    //        {
    //            currentColor.a -= changeSpeed * Time.deltaTime;
    //            if (currentColor.a <= minOpacity)
    //            {
    //                currentColor.a = minOpacity;
    //                increasingOpacity = true;
    //            }
    //        }

    //        // 変更した透明度を適用
    //        startText.color = currentColor;
    //        Debug.Log(startText.color);
    //    }
    //}
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
                timer.text = (playTime.ToString());
            }

        }

        //タイム延長エフェクト
        if (playTime > timeLastFrame && sizeEffectCoroutine == false)
        {
            Vector2 generatePos = timeGO.transform.position;
            generatePos.x += 0.5f;
            StartCoroutine(SizeEffect(timer, 100, 130));
            //GameObject textPrefab = (GameObject)Resources.Load("TextPrefab");
            //GameObject textObj = Instantiate(textPrefab, generatePos, Quaternion.identity);
        }
        timeLastFrame = playTime;
    }

    IEnumerator SizeEffect(Text text, int originalSize, int maxSize)
    {
        sizeEffectCoroutine = true;
        text.fontSize = originalSize;
        int n = 350;
        while (text.fontSize <= maxSize)
        {
            text.fontSize += (int)(n * Time.deltaTime);
            if (text.fontSize % 2 == 0) { yield return null; }
        }

        //yield return new WaitForSeconds(0.1f);

        while (text.fontSize >= originalSize)
        {
            text.fontSize -= (int)(n * Time.deltaTime);
            if (text.fontSize % 2 == 0) { yield return null; }
        }
        sizeEffectCoroutine = false;
    }
}

