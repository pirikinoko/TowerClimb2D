using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System;

public class Player : MonoBehaviour
{
    GameObject player;
    public Rigidbody2D rbody2D;
    [HideInInspector] public string animeState = "idle", wallName;
    [HideInInspector] public bool autoWallJump = true, onGround, legOnGround, wallflag = false, jumpFlag = false, onWall, isMoving = false, isAttacking = false, isCrouch = false, isSlide = false, speceKeyPressed = false;
    [HideInInspector] public int jumpCount = 0, lastNum;
    [SerializeField] GameObject attackCol;
    private Animator playerAnim;
    BoxCollider2D legCol2d, col2d;
    public static Vector2 characterDirection;
    public Tilemap tilemap;
    public Text DebugText;
    public static float avgSpeedY;
    public Vector2 defaultPos;
    public float speed, jumpForce;
    float Gravity = 3500, elapsedTime, wallJumpTime, attackSign, attackDuration = 1.0f, slideDuration, speedYGoal, lastTIme, updateTextPeriod;
   
    Vector3 latestPos, playerPos;
    Vector2 playerSpeed, defaultSize;
    float[] playerSpeedYRecord = new float[30];
    int calcCount = 0;
    const int nullNumber = -999;
    float[] speedDefault = new float[2], jumpForceDefault = new float[2];
    void Start()
    {
        lastTIme = 0;
        lastNum = 0;
        calcCount = 0;
        avgSpeedY = 0;
        updateTextPeriod = 0.1f;
        player = this.gameObject;
        col2d = this.GetComponent<BoxCollider2D>();
        playerAnim = this.GetComponent<Animator>();
        defaultSize = col2d.size;
        legCol2d = transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        //Rigidbodyを取得
        rbody2D = GetComponent<Rigidbody2D>();
        //初期位置の保存
        Transform myTransform = this.transform;
        latestPos = player.transform.position;
        this.transform.position = defaultPos;
        for (int i = 0; i < playerSpeedYRecord.Length; i++)
        {
            playerSpeedYRecord[i] = nullNumber;
        }
        speedDefault[0] = speed;
        speedDefault[1] = speed * 1.5f;
        jumpForceDefault[0] = jumpForce;
        jumpForceDefault[1] = jumpForce * 1.5f;
    }

    void Update()
    {
        //重力付与
        rbody2D.AddForce(transform.up * -Gravity * Time.deltaTime);
        Vector3 legPos = playerPos; legPos.y -= 0.12f;
        legCol2d.transform.position = legPos;
        legCol2d.offset = new Vector2(0, 0);
        if (isSlide)
        {
            legCol2d.offset = new Vector2(0.04f, 1.6f);
        }
        if (GameSystem.playable)
        {
            //getTiles();
            Move();
            Jump();
            Ctrl();
            PlayerSpeed();
            PlayAnim();
            PhysicalBuff();
            Attack();
        }
    }


    //接触時処理
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            if (!onGround)
            {
                onWall = true;
            }
        }
        for (int i = 0; i < 30; i++)
        {
            if (other.gameObject.name.Contains("-" + i))
            {
                //コンボを０にする処理 
                lastNum = i;
            }
        }

    }

    //床に触れている間ジャンプカウントリセット
    private void OnCollisionStay2D(Collision2D other)
    {
        float distance = 0.1f;
        int layerMask = 1 << LayerMask.NameToLayer("Default");
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.down, distance, layerMask);
        if (hit.collider == null )
        {
                string colname = other.gameObject.name;
                animeState = "OnWall";
                //最後にぶつかった壁の名前が当たった壁と違うとき壁ジャンプリセット
                if (other.gameObject.name != wallName) { jumpCount = 1; }
                if (other.gameObject.name != "None")
                {
                    wallName = other.gameObject.name;
                }

                if (playerSpeed.y == 0)
                {

                }
        }
        else 
        {
            if (legOnGround)
            {
                if (isMoving == false && !speceKeyPressed) { animeState = "idle"; }
                wallName = "Surface";
                jumpCount = 0;
                wallflag = false;
                onGround = true;
            }
        }

    }
    private void OnCollisionExit2D(Collision2D other)
    {
        //床から離れたときジャンプした判定
        if (other.gameObject.CompareTag("Surface"))
        {
            jumpCount = 1;
            onGround = false;
        }
        //壁から離れたとき
        if (other.gameObject.CompareTag("Wall"))
        {
            wallflag = true;
            onWall = false;
            animeState = "jumpUp";
        }
    }

    void PlayerSpeed()
    {
        //プレイヤー速度取得
        playerPos = this.transform.position;
        playerSpeed = ((playerPos - latestPos) / Time.deltaTime);
        latestPos = playerPos;
        ////平均速度
        //if ((int)TimeScript.pastTime > lastTIme) //１秒ごとの速度を配列に記録していく（最大10個）
        //{
        //    playerSpeedYRecord[calcCount] = playerSpeed.y;
        //    lastTIme++;
        //    calcCount++;
        //    if (calcCount == playerSpeedYRecord.Length) { calcCount = 0; }
        //}
        ////直近30秒間で進んだ距離を計算
        //speedYGoal = 0;
        //for (int i = 0; i < playerSpeedYRecord.Length; i++)
        //{
        //    if (playerSpeedYRecord[i] != nullNumber)
        //    {
        //        speedYGoal += playerSpeedYRecord[i];
        //    }
        //}
        //if (speedYGoal == 0 || (int)TimeScript.pastTime < 10)
        //{
        //    avgSpeedYText.text = "";
        //    return;
        //}
        //if ((int)TimeScript.pastTime < 30)
        //{
        //    speedYGoal *= 90 / (int)TimeScript.pastTime;
        //}
        //else
        //{
        //    speedYGoal *= 3;
        //}
        ////流動的に数値を表示
        //if (avgSpeedY < speedYGoal)
        //{
        //    avgSpeedY += speedYGoal * Time.deltaTime;
        //}
        //else if (avgSpeedY > speedYGoal)
        //{
        //    avgSpeedY -= speedYGoal * Time.deltaTime;
        //}
        ////0.1秒ごとにテキスト更新 
        //updateTextPeriod -= Time.deltaTime;
        //if (updateTextPeriod < 0)
        //{
        //    avgSpeedYText.text = String.Format("{0:##.#}", avgSpeedY) + "m/90s";
        //    updateTextPeriod = 0.1f;
        //}





        //落下速度調整      
        if (playerSpeed.y < -3.5f)
        {
            rbody2D.velocity = new Vector2(0, -3.5f);
        }

    }
    void Move()
    {
        //プレイヤーの位置を取得
        Vector2 position = transform.position;
        if (isSlide)
        {
            speed = 1.3f;
            Vector2 direction = gameObject.transform.localScale;
            position.x -= (direction.x * 10) * speed * Time.deltaTime;
            transform.position = position;
            return;
        }
        else if (isAttacking)
        {
            speed = 0.7f;
        }
        else
        {
            speed = 1.0f;
        }
        //ADキーで移動
        if (Input.GetKey(KeyCode.A))
        {
            position.x -= speed * Time.deltaTime;
            if (speceKeyPressed == false && playerSpeed.y < 0)
            {
                if (onWall) { rbody2D.velocity = new Vector2(0, -0.5f); }
            }

            if (jumpCount < 1 && !onWall && !(isSlide))//アニメーション
            {
                animeState = "run";
            }
            if (onGround) { isMoving = true; }
            if (!isAttacking && !isSlide)
            {
                Vector2 direction = new Vector2(0.1f, 0.1f);
                gameObject.transform.localScale = direction;
            }


        }
        else if (Input.GetKey(KeyCode.D))
        {
            position.x += speed * Time.deltaTime;
            if (speceKeyPressed == false && playerSpeed.y < 0)
            {
                if (onWall) { rbody2D.velocity = new Vector2(0, -0.5f); }
            }

            if (jumpCount < 1 && !onWall && !(isSlide))//アニメーション
            {
                animeState = "run";
            }
            if (onGround) { isMoving = true; }
            if (!isAttacking && !isSlide)
            {
                Vector2 direction = new Vector2(-0.1f, 0.1f);
                gameObject.transform.localScale = direction;
            }
        }

        if (isMoving && (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)))
        {
            isMoving = false;
            animeState = "idle";
        }

        transform.position = position;
    }

    void Jump()
    {   //地面にいるとき||壁に触れているとき
        if (!isSlide && (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0 && !onWall && playerSpeed.y == 0) || (!isSlide && Input.GetKeyDown(KeyCode.Space) && jumpCount == 1 && (wallflag || onWall)))
        {
            /*
            if(jumpCount == 1 && autoWallJump)
            {
                if(wallName.Contains("Left")){rbody2D.velocity = new Vector2(0.8f, jumpForce);}
                else{rbody2D.velocity = new Vector2(-0.8f, jumpForce);}
            }
            else { rbody2D.velocity = new Vector2(0f, jumpForce); }
            */
            rbody2D.velocity = new Vector2(0f, jumpForce);
            jumpCount++;
            speceKeyPressed = true;
        }
        if (!onGround && !onWall)
        {
            if (playerSpeed.y > 0)
            {
                animeState = "jumpUp";
            }
            else
            {
                animeState = "jumpDown";
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            speceKeyPressed = false;
            if (playerSpeed.y > 0)
            {
                rbody2D.velocity = new Vector2(0, 0.5f);
            }
        }

        //壁ジャンプ可能時間
        if (wallflag)
        {
            wallJumpTime += Time.deltaTime;
        }
        if (wallJumpTime > 0.1f)
        {
            wallJumpTime = 0;
            wallflag = false;
        }

    }

    void Attack() //攻撃処理
    {
        Vector2 colPos = this.transform.position;
        if ((Input.GetKeyDown(KeyCode.N) || Input.GetMouseButtonDown(0)) && !(isAttacking) && !isSlide)
        {
            isAttacking = true;
            playerAnim.SetTrigger("attack");
        }

        if (isAttacking)
        {
            attackDuration -= Time.deltaTime;
        }


        if (attackDuration < 0)
        {
            isAttacking = false;
            animeState = "idle";
            attackDuration = 1.0f;
        }

        if (0.1f < attackDuration && attackDuration < 0.40f)   //攻撃の当たり判定ON
        {
            attackCol.gameObject.SetActive(true);
        }
        else { attackCol.gameObject.SetActive(false); }

        colPos.x += gameObject.transform.localScale.x * -1f;
        colPos.y -= 0.05f;
        attackCol.transform.position = colPos;

    }

    void Ctrl()
    {
        if (!isAttacking && onGround && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.M)))
        {
            col2d.size = new Vector2(1, 1.8f);
            if (playerSpeed.x < 0 || 0 < playerSpeed.x)
            {
                isSlide = true;
                slideDuration = 0f;
            }
            else
            {
                isCrouch = true;
                animeState = "crouch";
            }
        }
        if (isCrouch && Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.M))
        {
            col2d.size = defaultSize;
            isCrouch = false;
            animeState = "idle";
        }

        if (isCrouch)
        {
            col2d.size = new Vector2(1, 1.8f);
        }


        if (isSlide)
        {
            slideDuration += Time.deltaTime;
            animeState = "sliding";
            if (slideDuration > 0.5f || (!onGround && slideDuration > 0.15f))
            {
                slideDuration = 0;
                col2d.size = defaultSize;
                isSlide = false;
                animeState = "idle";
            }
        }
    }
    void getTiles()
    {
        // プレイヤーの現在位置を取得
        Vector3Int playerCellPosition = tilemap.WorldToCell(transform.position);
        playerCellPosition.y -= 1;
        // プレイヤーが接触しているタイルを取得
        TileBase tile = tilemap.GetTile(playerCellPosition);

        if (tile != null)
        {
            // タイルが存在する場合、タイルのIDを取得
            int tileID = tile.GetInstanceID();
        }
    }

    void PhysicalBuff()
    {
        if (BuffManagement.buffTrigger[0])
        {
            //speed = speedDefault[1];
            //jumpForce = jumpForceDefault[1];
        }
        else 
        {
            //speed = speedDefault[0];
            //jumpForce = jumpForceDefault[0];
        }
    }
    void PlayAnim()
    {
        playerAnim.SetBool("idle", false);
        playerAnim.SetBool("run", false);
        playerAnim.SetBool("jumpUp", false);
        playerAnim.SetBool("jumpDown", false);
        playerAnim.SetBool("OnWall", false);
        playerAnim.SetBool("sliding", false);
        playerAnim.SetBool(animeState, true);
    }
}



