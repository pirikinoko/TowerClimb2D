using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameSystem : MonoBehaviour
{
    public Text scoreText, comboText, resultScoreTx, resultTimeTx, rankTx;
    public GameObject scoreTextGO, comboTextGO, player, resultPanel,goalLine;
    public static int combo;
    int scoreDisplay;
    public static float resultTime, resultScore , score;
    int stage1Goal = 5, lastCombo = 0;
    string rank;
    public static string gameState = "Playing";
    public static bool playable = true;
    Vector2 goalPos; //ゴールした時のプレイヤー位置
    // Start is called before the first frame update
    void Start()
    {
        playable = false;
        gameState = "Playing";
        score = 0;
        resultScore = 0;
        resultTime = 0;
        combo = 0;
        lastCombo = 0;
        player = GameObject.Find("Player");
        scoreTextGO = GameObject.Find("Score");
        comboTextGO = GameObject.Find("Combo");
        resultPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ScoreDisplay();
        TimeUp();
        Effect();
        scoreText.text = scoreDisplay.ToString();
        comboText.text = combo.ToString();
    }
    void FixedUpdate()
    {
        ShowResult();
    }

    void ShowResult()
    {
        if (gameState == "TimeUp")
        {
            resultPanel.gameObject.SetActive(true);
            player.transform.position = goalPos;
            int SECount1 = 0;
            int SECount2 = 0;
            resultTimeTx.text = String.Format("{0:##.#}", resultTime);  //小数点第二位以下を非表示
            resultScoreTx.text = String.Format("{0:####}", resultScore);  //整数のみ表示
            if (resultTime < TimeScript.pastTime)
            {
                GainResultTime();
            }         
            else if (resultScore < score)
            {
                GainResultScore();
            }     
        }
      
    }
    void TimeUp()
    {
        if(TimeScript.playTime < 0)
        {
            gameState = "TimeUp";
            playable = false;
            TimeScript.playTime = 0;
        }
    }
    void GainResultTime()
    {
        resultTime += TimeScript.pastTime / 1500;
        if (resultTime >= TimeScript.pastTime)
        {
            SoundEffect.BunTrigger = true;
        }
    }
    void GainResultScore()
    {
        resultScore += score / 1500;
        if (resultScore == score)
        {
            SoundEffect.BunTrigger = true;
        }
    }

    void ScoreDisplay()
    {
        if(scoreDisplay < score)
        {
            scoreDisplay ++;
        }
    }
    private Coroutine sizeEffectCoroutine;
    void Effect()
    {
        if (combo != lastCombo)
        {
            if (sizeEffectCoroutine != null)
            {
                // 既に実行中のコルーチンがあれば停止させる
                StopCoroutine(sizeEffectCoroutine);
            }
            StartCoroutine(SizeEffect(comboText, 150, 220));
            lastCombo = combo;
        }

    }
    IEnumerator SizeEffect(Text text, float originalSize, float maxSize)
    {
        float speed = 5.0f;
        while (text.fontSize < maxSize)
        {
            text.fontSize = (int)Mathf.Lerp(text.fontSize, maxSize + 10, speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        while (text.fontSize > originalSize)
        {
            text.fontSize = (int)Mathf.Lerp(text.fontSize, originalSize - 10, speed * Time.deltaTime);
            yield return null;
        }
        sizeEffectCoroutine = null;
    }


}

