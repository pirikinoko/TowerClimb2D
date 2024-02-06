using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
public class Collision : MonoBehaviour
{
    int count = 0;
    float buffMulti, addTimeEnemy = 2, addTimeCol = 0.3f;
    const int scoreBased = 10;
    SoundEffect soundEffect;
    // Start is called before the first frame update
    void Start()
    {
    
        soundEffect = GameObject.Find("AudioSystem").GetComponent<SoundEffect>();
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
                    GameObject scoreEffect = (GameObject)Resources.Load("ScoreEffect");
                    Instantiate(scoreEffect, other.transform.position, Quaternion.identity);
                    this.gameObject.GetComponent<Light2D>().intensity = 0;
                    soundEffect.PlaySE(2);
                    GameSystem.combo++;
                    GameSystem.score += scoreCalc();
                    TimeScript.elapsedTime += addTimeCol;
                    SetFeedBackPos(other.transform.position);
                    ScoreFeedBack.scoreDiff = scoreCalc();
                    count++;
                }

            }
           
            if (this.gameObject.CompareTag("Surface"))
            {
                if(count == 0)
                {
                    GameObject scoreEffect = (GameObject)Resources.Load("ScoreEffect");
                    Instantiate(scoreEffect, other.transform.position, Quaternion.identity);
                    this.gameObject.GetComponent<Light2D>().intensity = 0;
                    soundEffect.PlaySE(2);
                    GameSystem.combo++;
                    GameSystem.score += scoreCalc();
                    TimeScript.elapsedTime += addTimeCol;
                    SetFeedBackPos(other.transform.position);
                    ScoreFeedBack.scoreDiff = scoreCalc();
                    count++;
                }
            }
            if (this.gameObject.CompareTag("Enemy"))
            {
               GameSystem.score -= 5;
               GameSystem.combo = 0;
               soundEffect.PlaySE(0);
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

        if (this.gameObject.name.Contains("Event") && other.gameObject.CompareTag("Player"))
        {
            EventSlot eventSlot = GameObject.Find("SlotItem").GetComponent<EventSlot>();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(eventSlot.RollSlot(this.gameObject.name));
        }

    }

    float scoreCalc()
    {
        float heightBonus = 1 + (GenerateStage.playerHeight / 30);
        buffMulti = 1.0f;
        if (BuffManagement.buffTrigger[0])
        {
            buffMulti = 3.0f;
        }
        return GameSystem.scoreBase * (GameSystem.combo / GameSystem.scorePer) * (heightBonus) * buffMulti;
    }

    void SetFeedBackPos(Vector2 collisionPos)
    {
        Vector2 posTmp = collisionPos;
        posTmp.x += 0.2f;
        posTmp.y += 0.2f;
        ScoreFeedBack.feedBackPos = posTmp;
    }
}

