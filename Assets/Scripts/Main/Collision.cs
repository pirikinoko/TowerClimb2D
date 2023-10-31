﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Collision : MonoBehaviour
{
    int count = 0;
    float buffMulti;
    const int scoreBased = 10;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
       
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("Wall"))
            {
                if(count == 0)
                { 
                    this.gameObject.GetComponent<Light2D>().intensity = 0;
                    SoundEffect.sound3Trigger = true;
                    GameSystem.combo++;
                    GameSystem.score += scoreCalc();
                    TimeScript.elapsedTime += 1;
                    SetFeedBackPos(other.transform.position);
                    ScoreFeedBack.scoreDiff = scoreCalc();
                    count++;
                }

            }
           
            if (this.gameObject.CompareTag("Surface"))
            {
                if(count == 0)
                {  
                    this.gameObject.GetComponent<Light2D>().intensity = 0;
                    SoundEffect.sound3Trigger = true;
                    GameSystem.combo++;
                    GameSystem.score += scoreCalc();
                    TimeScript.elapsedTime += 1;
                    SetFeedBackPos(other.transform.position);
                    ScoreFeedBack.scoreDiff = scoreCalc();
                    count++;
                }
            }
            if (this.gameObject.CompareTag("Enemy"))
            {
               GameSystem.score -= 5;
               GameSystem.combo = 0;
               SoundEffect.sound1Trigger = true;
               Debug.Log(other.gameObject.name + "と衝突しました");
               Destroy(this.gameObject);
            }
            ScoreFeedBack.diffBeforeMulti = ScoreFeedBack.scoreDiff / buffMulti;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(this.gameObject.name.Contains("Check"))
        {
            int objNumber = 0;
            if(other.gameObject.name.Contains("Wall"))
            {
                objNumber += 4;
            }
            for(int i = 0; i < 4; i++)
            {
                if(other.gameObject.name.Contains((i + 1).ToString()))
                {
                    objNumber += i;
                }
            }
            GenerateStage.collisionPos[objNumber] = this.gameObject.transform.position.x;
        }
        
    }

    float scoreCalc()
    {
        float scoreMultiBySpeed = Player.avgSpeedY / 16;
        if (scoreMultiBySpeed < 1.0f) { scoreMultiBySpeed = 1.0f; }
        buffMulti = 1.0f;
        if (BuffManagement.buffTrigger[0])
        {
            buffMulti = 3.0f;
        }
        return scoreBased  * (GameSystem.combo / 5) * (scoreMultiBySpeed) * buffMulti;
    }

    void SetFeedBackPos(Vector2 collisionPos)
    {
        Vector2 posTmp = collisionPos;
        posTmp.x += 0.2f;
        posTmp.y += 0.2f;
        ScoreFeedBack.feedBackPos = posTmp;
    }
}

