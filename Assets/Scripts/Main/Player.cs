using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody2D rbody2D;
    GameObject player;
    //重力
    float Gravity = 11000;
    //時間計測用
    float elapsedTime; //ジャンプ時間用
    float wallJumpTime; //壁ジャンプに余裕持たせる
    bool wallflag = false;
    bool counter_flag = false;
    bool isMoving = false;

    //どの種類の壁に触れているか
    private string judgeWall = "None";
    //触れた壁の名前
    string wallName;

    //ジャンプ最大値の秒数
    float maxJumpHeight = 0.34f;
    //ジャンプ回数
    private float jumpCount = 0;
    //ジャンプ力
    private float jumpForce = 3.8f;
    //プレイヤー移動速度
    float speed = 50, maxSpeed = 1.7f;
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
    //animation
    [SerializeField]
    private Animator playerAnim;

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
            playerAnim.SetBool("attack", false);
            if (Input.GetMouseButtonDown(0))
            {
                playerAnim.SetBool("attack", true);
            }
        }
    }


    //接触時処理
    private void OnCollisionEnter2D(Collision2D other)
    {
        //壁または床にぶつかったらジャンプリセット
        if (other.gameObject.CompareTag("RightWall") || other.gameObject.CompareTag("LeftWall") || other.gameObject.CompareTag("AltWall"))
        {
            string colname = other.gameObject.name;
            judgeWall = colname.Substring(0, colname.Length - 4);
            //最後にぶつかった壁の名前が当たった壁と違うとき壁ジャンプリセット
            if (other.gameObject.name != "None")
            {
                wallName = other.gameObject.name;
                jumpCount = 1;
            }
        }

        if (other.gameObject.CompareTag("Surface"))
        {
            jumpCount = 0;
            judgeWall = "None";
            //ジャンプ時間計測(停止)
            if (counter_flag == true)
            {
                counter_flag = !counter_flag;
                elapsedTime = 0;
            }
            playerAnim.SetBool("jump", false);
        }
    }

    //床に触れている間ジャンプカウントリセット
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Surface"))
        {
            jumpCount = 0;
            wallflag = false;

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
        }
    }

    void PlayerSpeed()
    {
        //プレイヤー速度取得
        playerPos = this.transform.position;
        playerspeed = ((playerPos - latestPos) / Time.deltaTime);
        latestPos = playerPos;
        //落下速度調整      
        if (playerspeed.y < -4f)
        {
            rbody2D.velocity = new Vector2(0, -4f);
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
                if (judgeWall != "None") { rbody2D.velocity = new Vector2(0, -0.8f);   }               
            }

            if (jumpCount < 1 && judgeWall == "None")//アニメーション
            {
                playerAnim.SetBool("run", true);
            }
            isMoving = true;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (movement < maxSpeed)
            {
                movement += speed * Time.deltaTime;
            }

            if (spaceKeyState = false && elapsedTime < maxJumpHeight + 0.1f)
            {
                if (judgeWall != "None") { rbody2D.velocity = new Vector2(0, -0.8f); }
            }

            if (jumpCount < 1 && judgeWall == "None")//アニメーション
            {
                playerAnim.SetBool("run", true);
            }
            isMoving = true;
        }

        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            isMoving = false;
        }


        //movementの符号取得
        float sign = Mathf.Sign(movement);

        characterDirection = new Vector2(-sign * 0.1f, 0.1f);
        if (movement != 0) { gameObject.transform.localScale = characterDirection; } //動いていないとき(movement = 0)は右向きになってしまうので反映しない

        //徐々に減速
        sign *= -1;　//減速なのでmovementの逆の符号を加える
        if(movement != 0)
        {
            if (isMoving) { movement += sign * (stopper * Time.deltaTime); }
            else { movement += sign * ((stopper * 20) * Time.deltaTime); } //移動キーを離すとさ
        }
       

        //デッドゾーン
        if (-deadZone < movement && movement < deadZone)
        {
            movement = 0;
        }

        
        //アニメーションOFF
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || jumpCount >= 1)
        {
            playerAnim.SetBool("run", false);
        }
        position.x += movement * Time.deltaTime;
        transform.position = position;
    }

    void Jump()
    {
        //ジャンプ
        //キーが押されたとき&&地面にいるとき
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1 && judgeWall == "None")
        {
            rbody2D.velocity = new Vector2(0f, jumpForce);
            jumpCount++;
            //時間計測開始
            counter_flag = true;
            spaceKeyState = true;
            //アニメーション
            playerAnim.SetBool("jump", true);
        }
        //壁ジャンプ
        else if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2 && judgeWall != "None")
        {
            rbody2D.velocity = new Vector2(0f, jumpForce);
            jumpCount++;
            //時間計測開始
            counter_flag = true;
            spaceKeyState = true;
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
                    rbody2D.velocity = new Vector2(0, 0);
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
            playerAnim.SetBool("jump", false);

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

}

     

