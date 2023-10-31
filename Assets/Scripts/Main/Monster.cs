using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Player player;
    float Speed = 0.5f, jumpForce = 1.5f;
    float buffMulti;
    const int scoreBased = 10;
    public float rightLimit, leftLimit;
    Rigidbody2D rb2D;
    Vector2 characterDirection, MonsterPos, localScale, defaultPos;

    bool onSurface = false;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = this.transform.position;
        MonsterPos = this.transform.position;
        Vector2 characterDirection = new Vector2(0.01f, 0.01f);
        rb2D = this.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per dadaDadad
    void Update()
    {
        MonsterPos.x += Speed * Time.deltaTime;
        if (MonsterPos.x >= defaultPos.x + rightLimit || MonsterPos.x < defaultPos.x - leftLimit)
        {
            Turn();
        }
        int rnd = Random.Range(0, 50);
        if(rnd == 0){ Jump();}
        Vector2 nowvelocity = rb2D.velocity;
        this.transform.position = MonsterPos;
        rb2D.velocity = nowvelocity;
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
            SoundEffect.sound3Trigger = true;
            Destroy(this.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        onSurface = true;
    }

    void Jump()//ジャンプ
    {
        if (onSurface)
        {
            rb2D.velocity = new Vector2(0, jumpForce);
            onSurface = false;

        }
    }
    void Turn()
    {      
        Speed *= -1;
        MonsterPos.x += Speed * Time.deltaTime;
        Vector2  characterDirection = gameObject.transform.localScale;
        characterDirection.x *= -1;
        gameObject.transform.localScale = characterDirection; 
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

}
