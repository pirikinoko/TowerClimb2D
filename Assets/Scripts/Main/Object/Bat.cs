using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class Bat : MonoBehaviour
{
    Player player;
    float speed = 0.5f, changePeriod;
    float buffMulti;
    const int scoreBased = 10;
    float rightLimit, leftLimit;
    Rigidbody2D rb2D;
    Vector2 MonsterPos, localScale, defaultPos, latestPos, monsterVector;
    GenerateStage generateStage;
    SoundEffect soundEffect;
   
    void Start()
    {
        defaultPos = this.transform.position;
        MonsterPos = this.transform.position;
        leftLimit = 1;
        rightLimit = 1;
        changePeriod = 0.7f;
        rb2D = this.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Player>();
        generateStage = GameObject.Find("GameSystem").GetComponent<GenerateStage>();
        soundEffect = GameObject.Find("AudioSystem").GetComponent<SoundEffect>();
    }

    // Update is called once per dadaDadad
    void Update()
    {
        Move();
        Disapper();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wepon"))
        {

            GameSystem.score += scoreCalc();
            SetFeedBackPos(other.transform.position);
            ScoreFeedBack.scoreDiff = scoreCalc();
            TimeScript.elapsedTime += 3.0f;
            GameSystem.combo++;
            soundEffect.PlaySE(2);
            Destroy(this.gameObject);

            Vector3 effectPos = this.transform.position;
            effectPos.y += 0.2f;
            GameObject scoreEffect = (GameObject)Resources.Load("ScoreEffect");
            Instantiate(scoreEffect, effectPos, Quaternion.identity);
        }
    }

   
    void OnCollisionStay2D(Collision2D other)
    {
    

    }

    private void Move()
    {
        //MonsterPos = this.transform.position;
        //MonsterPos.x += speed * Time.deltaTime;
        //this.transform.position = MonsterPos;
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
        return scoreBased * (GameSystem.combo / 5) * (scoreMultiBySpeed) * buffMulti;
    }

    void SetFeedBackPos(Vector2 collisionPos)
    {
        Vector2 posTmp = collisionPos;
        posTmp.x += 0.2f;
        posTmp.y += 0.2f;
        ScoreFeedBack.feedBackPos = posTmp;
    }


    void Disapper()
    {
        GameObject player = GameObject.Find("Player");
        Vector2 playerPos = player.transform.position;
        if (playerPos.y - MonsterPos.y > 6)
        {
            Destroy(this.gameObject);
        }
    }
}
