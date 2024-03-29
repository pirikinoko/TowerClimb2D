﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameSystem : MonoBehaviour
{
    public Text scoreText, comboText, resultScoreTx, resultTimeTx, rankTx;
    public GameObject scoreTextGO, comboTextGO, player, resultPanel;
    float scoreDisplay, lastCombo = 0, highScore;
    public static float currentTime, resultScore , score, combo, scoreBase = 10, scorePer = 10;
    int stage1Goal = 5;
    string rank;
    public static string gameState = "Playing";
    public static bool playable = true;
    Vector2 goalPos; //ゴールした時のプレイヤー位置
    GenerateStage generateStage;
    SoundEffect soundEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        playable = false;
        gameState = "Playing";
        score = 0;
        resultScore = 0;
        currentTime = 0;
        combo = 0;
        lastCombo = 0;
        player = GameObject.Find("Player");
        scoreTextGO = GameObject.Find("Score");
        comboTextGO = GameObject.Find("Combo");
        resultPanel.gameObject.SetActive(false);
        generateStage = GameObject.Find("GameSystem").GetComponent<GenerateStage>();
        soundEffect = GameObject.Find("AudioSystem").GetComponent<SoundEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreDisplay();
        GameOver();
        Effect();
        if (playable)
        {
            scoreText.text = "Score:" + String.Format("{0:####}", scoreDisplay);
        }
        else
        {
            scoreText.text = "HighScore:" + String.Format("{0:####}", highScore);
        }
        comboText.text = "x" + String.Format("{0:####}", combo);
    }
    void FixedUpdate()
    {
        StartCoroutine(ShowResult());
    }

    IEnumerator ShowResult()
    {
        if (gameState == "Result")
        {
            resultPanel.gameObject.SetActive(true);
            rankTx.text = null;
            player.transform.position = goalPos;
            int SECount1 = 0;
            int SECount2 = 0;
            resultTimeTx.text = String.Format("{0:##.#}", currentTime);  //小数点第二位以下を非表示
            resultScoreTx.text = String.Format("{0:####}", resultScore);  //整数のみ表示
            while(currentTime < TimeScript.pastTime)
            {
                currentTime += TimeScript.pastTime / 1500;
                if (currentTime >= TimeScript.pastTime)
                {
                    soundEffect.PlaySE(1);
                }
                yield return new WaitForSeconds(0.3f);
            }
            while (resultScore < score)
            {
                resultScore += score / 1500;
                if (resultScore == score)
                {
                    soundEffect.PlaySE(0);
                }
               yield return new WaitForSeconds(0.3f);
            }     
             yield return new WaitForSeconds(1f);
            if(score > 10000) 
            {
                rankTx.text = "A";
            }
            else { rankTx.text = "B"; }
            gameState = "Over";
            if(score > highScore)
            {
                highScore = score;
            }
        }
      
    }
    void GameOver()
    {
        Vector2 playerPos = player.transform.position;
        if(gameState == "Playing")
        {
            if(TimeScript.playTime < 0 || playerPos.y < generateStage.deadLine)
            {
                gameState = "Result";
                playable = false;
                TimeScript.playTime = 0;
            }
        }
    }

    void ScoreDisplay()
    {
        if(scoreDisplay < score)
        {
            scoreDisplay += score / 300;
        }
        else{ scoreDisplay = score; }
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
    IEnumerator SizeEffect(Text text, int originalSize, int maxSize)
    {
        float speed = 5.0f;
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
        sizeEffectCoroutine = null;
    }

    IEnumerator SizeEffet2(Text text, int originalSize, int maxSize)
    {
        float speed = 5.0f;
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
        sizeEffectCoroutine = null;
    }

  


}

