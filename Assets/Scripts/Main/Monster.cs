using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class Monster : MonoBehaviour
{
    Player player;
    float speed = 0.5f, jumpForce = 1.5f, jumpCD;
    float buffMulti;
    const int scoreBased = 10;
    float rightLimit, leftLimit , posGap;
    Rigidbody2D rb2D;
    Vector2 characterDirection, MonsterPos, localScale, defaultPos, latestPos, monsterVector;
    string colFloorName, saveFloorName;
    int colFloorLength = 0;
    bool onSurface = false;
    GenerateStage generateStage;
    // Start is called before the first frame update
    void Start()
    {
        saveFloorName = "Default";
        defaultPos = this.transform.position;
        MonsterPos = this.transform.position;
        Vector2 characterDirection = new Vector2(0.01f, 0.01f);
        jumpCD = Random.Range(0, 50);
        leftLimit = 1;
        rightLimit = 1;
        rb2D = this.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Player>();
        generateStage = GameObject.Find("GameSystem").GetComponent<GenerateStage>();
    }

    // Update is called once per dadaDadad
    void Update()
    {
    

        MonsterPos = this.transform.position;
        MonsterPos.x += speed * Time.deltaTime;
        if (MonsterPos.x >= defaultPos.x + rightLimit || MonsterPos.x < defaultPos.x - leftLimit)
        {
            Turn();
        }
        jumpCD -= 10 * Time.deltaTime;
        if(jumpCD < 0)
        {
            Jump();
            jumpCD = Random.Range(10, 50);
        }
        Vector2 nowvelocity = rb2D.velocity;
        this.transform.position = MonsterPos;
        Disapper();
        //rb2D.velocity = nowvelocity;

        //速度
        monsterVector = ((MonsterPos - latestPos) / Time.deltaTime);
        latestPos = MonsterPos;

        if (monsterVector.y < -3.5f)
        {
            rb2D.velocity = new Vector2(0, -3.5f);
        }
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

            Vector3 effectPos = this.transform.position;
            GameObject effectObj = (GameObject)Resources.Load("EnemyDissapear");
            Instantiate(effectObj, effectPos, Quaternion.identity);
        }
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.name == "Player") 
        {
              
            int rnd1 = Random.Range(-1200, 1200);
            Vector2 randomForce = new Vector2(rnd1, 0);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(randomForce);
            player.isDown = true;
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("Floor") && (saveFloorName != other.gameObject.name))
        {
            defaultPos = this.transform.position;
            saveFloorName = other.gameObject.name;
            posGap = this.transform.position.x - other.transform.position.x;
        }
        if (other.gameObject.name.Contains("Floor")) 
        {
            onSurface = true;
            colFloorName = other.gameObject.name;
            char fifthCharacter = colFloorName[5];
            colFloorLength = int.Parse(fifthCharacter.ToString());

            leftLimit = (generateStage.eachLength[0, colFloorLength - 1] / 2) + posGap;
            rightLimit = (generateStage.eachLength[0, colFloorLength - 1] / 2) - posGap;
            leftLimit = Mathf.Clamp(leftLimit, 0, generateStage.eachLength[0, colFloorLength - 1]);
            rightLimit = Mathf.Clamp(rightLimit, 0, generateStage.eachLength[0, colFloorLength - 1]);
            leftLimit *= 0.95f;
            rightLimit *= 0.95f;    
        }
        else 
        {
            leftLimit = 1.5f;
            rightLimit = 1.5f;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            int rnd1 = Random.Range(-3, 3);
            int rnd2 = Random.Range(-3, 3);
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rnd1, rnd2);
        }

        if (other.gameObject.name == "Surface") 
        {
            Destroy(this.gameObject);
        }
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
        speed *= -1;
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
