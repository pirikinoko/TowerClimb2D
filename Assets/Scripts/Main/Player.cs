using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    GameObject player;
    public Rigidbody2D rbody2D;
    [SerializeField] GameObject attackCol;
    [SerializeField]
    private Animator playerAnim;
    CapsuleCollider2D col2d;
    BoxCollider2D legCol2d;
    public static Vector2 characterDirection;
    public Tilemap tilemap;
    public Text DebugText;
    public Vector2 defaultPos;
    public float  speed = 1.2f, jumpForce = 1.7f;
    public string judgeWall = "None";
    float Gravity = 6000, elapsedTime, wallJumpTime, attackSign, attackDuration = 1.0f, slideDuration, maxJumpHeight = 0.34f;
    [HideInInspector] public string animeState = "idle", wallName;
    [HideInInspector] public bool onGround, legOnGround,  wallflag = false, jumpFlag = false, onWall, isMoving = false, isAttacking = false, slideFrag = false, spaceKeyState = false;
    [HideInInspector] public int jumpCount = 0;
    Vector3 latestPos , playerPos;
    Vector2 playerspeed;
 
    void Start()
    {
        player = this.gameObject;
        col2d = this.GetComponent<CapsuleCollider2D>();
        legCol2d = transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
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
        Vector3 legPos = playerPos; legPos.y -= 0.13f;
        legCol2d.transform.position = legPos;
        if (GameSystem.playable)
        {
            getTiles();
            Move();
            Jump();
            Slide();
            PlayerSpeed();
            PlayAnim();
            Attack();
        }
        DebugText.text = jumpCount.ToString();
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
    }

    //床に触れている間ジャンプカウントリセット
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            string colname = other.gameObject.name;
            judgeWall = colname.Substring(0, colname.Length - 4);
            animeState = "OnWall";
            //最後にぶつかった壁の名前が当たった壁と違うとき壁ジャンプリセット
            if (other.gameObject.name != wallName) { jumpCount = 1; }
            if (other.gameObject.name != "None")
            {
                wallName = other.gameObject.name;
            }
        }
        if (other.gameObject.CompareTag("Surface"))
        {
            if (legOnGround)
            {
                if (isMoving == false && !spaceKeyState) { animeState = "idle"; }
                judgeWall = "None";
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
        if (slideFrag)
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
            if (spaceKeyState == false && elapsedTime < maxJumpHeight + 0.1f)
            {
                if (judgeWall != "None") { rbody2D.velocity = new Vector2(0, -0.5f);   }               
            }

            if (jumpCount < 1 && judgeWall == "None" && !(slideFrag))//アニメーション
            {
                animeState = "run";
            }
            isMoving = true;
            if (!isAttacking && !slideFrag)
            {
                Vector2 direction = new Vector2(0.1f, 0.1f);
                gameObject.transform.localScale = direction;
            }
           

        }
         else if (Input.GetKey(KeyCode.D))
        {
            position.x += speed * Time.deltaTime;
            if (spaceKeyState == false && elapsedTime < maxJumpHeight + 0.1f)
            {
                if (judgeWall != "None") { rbody2D.velocity = new Vector2(0, -0.5f); }
            }

            if (jumpCount < 1 && judgeWall == "None" && !(slideFrag))//アニメーション
            {
                animeState = "run";
            }
            isMoving = true;
            if (!isAttacking && !slideFrag)
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
        
        transform.position = position;
    }

    void Jump()
    {   //地面にいるとき||壁に触れているとき
        if ((Input.GetKeyDown(KeyCode.Space) && jumpCount == 0 && judgeWall == "None") || (Input.GetKeyDown(KeyCode.Space) && jumpCount == 1 && judgeWall != "None"))
        {
            rbody2D.velocity = new Vector2(0f, jumpForce);
            jumpCount++;
            spaceKeyState = true; 
            //アニメーション
            animeState = "jump";
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceKeyState = false;
            if (playerspeed.y > 0)
            {
                rbody2D.velocity = new Vector2(0, 0.5f);
            }
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
        if (Input.GetMouseButtonDown(0) && !(isAttacking))
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

        if (0.15f < attackDuration && attackDuration < 0.43f)   //攻撃の当たり判定ON
        {
            attackCol.gameObject.SetActive(true);
        }
        else { attackCol.gameObject.SetActive(false); }

        colPos.x += gameObject.transform.localScale.x * -1f;
        colPos.y -= 0.05f;
        attackCol.transform.position = colPos;

    }

    void Slide()
    {
        if (isAttacking || !onGround) { return; }
        if (onGround && Input.GetKeyDown(KeyCode.LeftControl))
        {      
            slideFrag = true;
        }
        if (slideFrag)
        {
            animeState = "sliding";
            slideDuration += Time.deltaTime;
            col2d.size = new Vector2(1, 1.8f);
            if (slideDuration > 0.5f)
            {
                slideDuration = 0;
                col2d.size = new Vector2(1, 2.7f);
                col2d.offset = new Vector2(0.04f, 0.1f);
                slideFrag = false;
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
            Debug.Log("Tile ID: " + tileID);
        }
    }
    void PlayAnim()
    {
        playerAnim.SetBool("idle", false);
        playerAnim.SetBool("run", false);
        playerAnim.SetBool("jump", false);
        playerAnim.SetBool("OnWall", false);
        playerAnim.SetBool("sliding", false);
        playerAnim.SetBool(animeState, true);
    }
}

     

