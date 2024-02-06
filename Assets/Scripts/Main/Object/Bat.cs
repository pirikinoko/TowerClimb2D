using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class Bat : MonoBehaviour
{
    Player player;
    GameObject leftEye, rightEye;
    float maxSpeed = 120f, addSpeed = 30,changePeriod, rChangePeriod = 0.4f;
    float buffMulti;
    const int scoreBased = 10;
    float rightLimit, leftLimit, xSpeed, ySpeed;
    Rigidbody2D rb2D;
    Vector2 MonsterPos, localScale, defaultPos, latestPos, playerPos ,monsterVector;
    GenerateStage generateStage;
    SoundEffect soundEffect;
   
    void Start()
    {
        defaultPos = this.transform.position;
        MonsterPos = this.transform.position;
        leftLimit = 1;
        rightLimit = 1;
        changePeriod = rChangePeriod;
        Transform batTransform = this.transform;
        GameObject eyeObj = (GameObject)Resources.Load("BatEye");
        leftEye = Instantiate(eyeObj, this.transform.position, Quaternion.identity);
        rightEye = Instantiate(eyeObj, this.transform.position, Quaternion.identity);
        leftEye.transform.SetParent(batTransform);
        rightEye.transform.SetParent(batTransform);
        rb2D = this.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Player>();
        generateStage = GameObject.Find("GameSystem").GetComponent<GenerateStage>();
        soundEffect = GameObject.Find("AudioSystem").GetComponent<SoundEffect>();
    }

    // Update is called once per dadaDadad
    void Update()
    {
        playerPos = GameObject.Find("Player").transform.position;
        MonsterPos = this.transform.position;
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
        changePeriod -= Time.deltaTime;
        if(changePeriod < 0)
        {
            changePeriod = rChangePeriod;
            xSpeed = Random.Range(-maxSpeed, maxSpeed);
            ySpeed = Random.Range(-1* (maxSpeed - xSpeed), maxSpeed - xSpeed);
            //プレイヤーの方向に進みやすく
            if (MonsterPos.x > playerPos.x) { xSpeed -= addSpeed; }
            else { xSpeed += addSpeed; }
            if (MonsterPos.y > playerPos.y) { ySpeed -= addSpeed; }
            else { ySpeed += addSpeed; }
        }
        
        rb2D.velocity = new Vector2(xSpeed / 100, ySpeed / 100);
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
        return GameSystem.scoreBase * (GameSystem.combo / 5) * (scoreMultiBySpeed) * buffMulti;
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
