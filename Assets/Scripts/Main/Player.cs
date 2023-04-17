using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody2D rbody2D;
    GameObject player;
    //重力
    float Gravity = 7000;
    //時間計測用
    float elapsedTime; //ジャンプ時間用
    float wallJumpTime; //壁ジャンプに余裕持たせる
    bool wallflag = false;
    bool counter_flag = false;
    bool isMoving = false;

    //どの種類の壁に触れているか
    public string judgeWall = "None";
    //触れた壁の名前
    string wallName;

    //ジャンプ最大値の秒数
    float maxJumpHeight = 0.34f;
    //ジャンプ回数
    private float jumpCount = 0;
    //ジャンプ力
    public float jumpForce;
    //プレイヤー移動速度
    public float speed = 50, maxSpeed = 1.2f;
    float movement, stopper = 1.5f, deadZone = 0.1f;
    //プレイヤー速度計測用
    Vector3 latestPos;
    Vector2 playerspeed;
    Vector3 playerPos; //プレイヤー位置
    //デバッグ用テキスト
    public Text DebugText;
    //初期位置を入れておく用のpos
    public Vector2 defaultPos;
    //キャラクター向き
    public static Vector2 characterDirection;

    //スペースキーの状態
    bool spaceKeyState = false;

    //攻撃
    [SerializeField] GameObject attackCol;
    bool whileAttack = false;
    float attackSign, attackDuration = 1.0f;
    //animation
    [SerializeField]
    private Animator playerAnim;
    string animeState = "idle";

    // Update is called once per frame
    void Start()
    {
        movement = 0;
        player = this.gameObject;
        //Rigidbodyを取得
        rbody2D = GetComponent<Rigidbody2D>();
        //初期位置の保存
        Transform myTransform = this.transform;
        latestPos = player.transform.position;
        this.transform.position = defaultPos;
    }

    void Update()
    {
        //重力付与
        rbody2D.AddForce(transform.up * -Gravity * Time.deltaTime);
        if (GameSystem.playable)
        {
            Move();
            Jump();
            PlayerSpeed();
            PlayAnim();
            Attack();
        }
    }


    //接触時処理
    private void OnCollisionEnter2D(Collision2D other)
    {
       
        if (other.gameObject.CompareTag("Surface"))
        {
            //ジャンプ時間計測(停止)
            if (counter_flag == true)
            {
                counter_flag = false;
                elapsedTime = 0;
            }
            if (!isMoving)
            {
                animeState = "idle";
            }
        }
    }

    //床に触れている間ジャンプカウントリセット
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Surface"))
        {
            judgeWall = "None";
            wallName = "Surface";
            jumpCount = 0;
            wallflag = false;

        }
        //壁または床にぶつかったらジャンプリセット
        else if (other.gameObject.CompareTag("RightWall") || other.gameObject.CompareTag("LeftWall") || other.gameObject.CompareTag("AltWall"))
        {
            string colname = other.gameObject.name;
            judgeWall = colname.Substring(0, colname.Length - 4);
            animeState = "OnWall";
            //最後にぶつかった壁の名前が当たった壁と違うとき壁ジャンプリセット
            if(other.gameObject.name != wallName) { jumpCount = 0; }
            if (other.gameObject.name != "None")
            {
                wallName = other.gameObject.name;

            }
         
        }
   
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        //床から離れたときジャンプした判定
        if (other.gameObject.CompareTag("Surface"))
        {
            jumpCount = 1;
        }
        //壁から離れたとき床に触れた判定にする
        if (other.gameObject.CompareTag("RightWall") || other.gameObject.CompareTag("LeftWall") || other.gameObject.CompareTag("AltWall"))
        {
            //壁ジャンプ可能な時間に余裕持たせる
            wallflag = true;
            animeState = "jump";
          
        }
    }

    void PlayerSpeed()
    {
        //プレイヤー速度取得
        playerPos = this.transform.position;
        playerspeed = ((playerPos - latestPos) / Time.deltaTime);
        latestPos = playerPos;
        //落下速度調整      
        if (playerspeed.y < -3.5f)
        {
            rbody2D.velocity = new Vector2(0, -3.5f);
        }
    }
    void Move()
    {
        //プレイヤーの位置を取得
        Vector2 position = transform.position;

        //ADキーで移動
        if (Input.GetKey(KeyCode.A))
        {
            if (movement > -maxSpeed)
            {
                movement -= speed * Time.deltaTime;
            }
        
            if (spaceKeyState == false && elapsedTime < maxJumpHeight + 0.1f)
            {
                if (judgeWall != "None") { rbody2D.velocity = new Vector2(0, -0.5f);   }               
            }

            if (jumpCount < 1 && judgeWall == "None")//アニメーション
            {
                animeState = "run";
            }
            isMoving = true;
            if (!whileAttack)
            {
                Vector2 direction = new Vector2(0.1f, 0.1f);
                gameObject.transform.localScale = direction;
            }
           

        }
         else if (Input.GetKey(KeyCode.D))
        {

            if (movement < maxSpeed)
            {
                movement += speed * Time.deltaTime;
            }

            if (spaceKeyState == false && elapsedTime < maxJumpHeight + 0.1f)
            {
                if (judgeWall != "None") { rbody2D.velocity = new Vector2(0, -0.5f); }
            }

            if (jumpCount < 1 && judgeWall == "None")//アニメーション
            {
                animeState = "run";
            }
            isMoving = true;
            if (!whileAttack)
            {
                Vector2 direction = new Vector2(-0.1f, 0.1f);
                gameObject.transform.localScale = direction;
            }
        }

        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            isMoving = false;
            animeState = "idle";
        }


        movement = Mathf.Lerp(movement, 0, 20 * Time.deltaTime);

        
        //アニメーションOFF
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            animeState = "idle";
        }
        position.x += movement * Time.deltaTime;
        transform.position = position;
    }

    void Jump()
    {   //地面にいるとき||壁に触れているとき
        if ((Input.GetKeyDown(KeyCode.Space) && jumpCount < 1 && judgeWall == "None") || (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1 && judgeWall != "None"))
        {
            rbody2D.velocity = new Vector2(0f, jumpForce);
            jumpCount++;
            //時間計測開始
            counter_flag = true;
            spaceKeyState = true;
            //アニメーション
            animeState = "jump";
        }

        //キーが離されたときジャンプキャンセル
        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceKeyState = false;
            if (elapsedTime != 0)
            {
                //ジャンプ中にキーが離されたときジャンプキャンセル
                if (elapsedTime <= maxJumpHeight)
                {
                    rbody2D.velocity = new Vector2(0, 0.5f);
                }
                //時間計測停止
                if (counter_flag == true)
                {
                    counter_flag = !counter_flag;

                }
                //ジャンプ時間デバッグ&リセット
                //Debug.Log("ジャンプ時間： " + (elapsedTime).ToString());
                elapsedTime = 0;

            }

        }
        //計測時間の表示
        if (counter_flag == true)
        {
            elapsedTime += Time.deltaTime;
        }

        //壁ジャンプ可能時間
        if (wallflag)
        {
            wallJumpTime += Time.deltaTime;
        }
        if (wallJumpTime > 0.05f)
        {
            judgeWall = "None";
            wallJumpTime = 0;
            wallflag = false;
        }

    }

    void Attack()　//攻撃処理
    {
        Vector2 colPos = this.transform.position;
        if (Input.GetMouseButtonDown(0) && !(whileAttack))
        {
            whileAttack = true;
            playerAnim.SetTrigger("attack");
        }

        if (whileAttack)
        {
            attackDuration -= Time.deltaTime;
            maxSpeed = 0.8f;   //攻撃時、移動速度減少
        }
        else { maxSpeed = 1.1f; }

        if (attackDuration < 0)
        {
            whileAttack = false;
            animeState = "idle";
            attackDuration = 1.0f;
        }

        if (0.15f < attackDuration && attackDuration < 0.43f)   //攻撃の当たり判定ON
        {
            attackCol.gameObject.SetActive(true);
        }
        else { attackCol.gameObject.SetActive(false); }

        colPos.x += gameObject.transform.localScale.x * -1f;
        colPos.y -= 0.05f;
        attackCol.transform.position = colPos;

    }

    void PlayAnim()
    {
        playerAnim.SetBool("idle", false);
        playerAnim.SetBool("run", false);
        playerAnim.SetBool("jump", false);
        playerAnim.SetBool("OnWall", false);
        playerAnim.SetBool(animeState, true);
    }
}

     

