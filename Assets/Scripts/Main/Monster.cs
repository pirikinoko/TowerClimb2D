using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    float Speed = 0.5f, jumpForce = 5f;
    Rigidbody2D rb2D;
    Vector2 characterDirection, MonsterPos, localScale;

    bool onSurface = false;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 characterDirection = new Vector2(0.01f, 0.01f);
        MonsterPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MonsterPos.x += Speed * Time.deltaTime;
        this.transform.position = MonsterPos;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Turn") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("LeftWall") || other.gameObject.CompareTag("RightWall"))
        {
            Speed *= -1;
            Turn(); 
        }
        if (other.gameObject.CompareTag("Wepon"))
        {

                GameSystem.score += 15 * GameSystem.combo / 5;
                GameSystem.combo++;
                SoundEffect.KirarinTrigger = true;
                Destroy(this.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        onSurface = true;
    }

    void Jump()　//ジャンプ
    {
        if (onSurface)
        {
            rb2D.velocity = new Vector2(0, jumpForce);
            onSurface = false;
        }
    }
    void Turn()
    {
        Vector2  characterDirection = gameObject.transform.localScale;
        characterDirection.x *= -1;
        gameObject.transform.localScale = characterDirection; 
    }


}
