 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
public class GenerateStage : MonoBehaviour
{
    [SerializeField] Tilemap castleBack, castleTile, skyTile;
    [SerializeField] Tile backBrick, sideWall;
    [SerializeField] GameObject player, checkLine;
    [SerializeField] GameObject[] frames;
    [SerializeField] Text heightText;
    GameObject[] obj = new GameObject[objUnit];
    GameObject[] slimes = new GameObject[5];
    GameObject[] bats = new GameObject[10];
    GameObject[] objForCheckLength;
    const int Floor = 0, Wall = 1, Right = 0, Left = 1, objUnit = 30, nullNumber = 99999;
    const float rightLimit = 1.8f, leftLimit = -2.1f;
    string [,] objNames = { { "Floor1", "Floor2", "Floor3", "Floor4" }, { "Wall1", "Wall2", "Wall3" , "Wall4"} };
    public float  [,] eachLength = new float[2,4];
    float xMax = 0, xMin = 0, yMax = 0, yMin = 0, playerYPrev, sizeX, sizeY, posX = -10, posY = 0, difficulty = 30, lastEItemY;
    bool[] objActive = new bool[objUnit];
    Vector3[] objPos = new Vector3[objUnit];
    Vector2 checkLinePos;
    public float deadLine { get; set; }
    public static int playerHeight;
    int[] objectType = new int[objUnit];
    int currentObj = 0, prev, prev2, count = 0, target = 0, tileY, objLength, objDirection = 0, batCount , batMax = 10,slimeCount, slimeMax = 5, startCount, deleteDuration = 5, startHeight, currentHeight;
    public static float[] collisionPos = new float[30];
    // Start is called before the first frame update
    void Start()
    {
        //生成するオブジェクトの長さを測る
        int length1 = objNames.GetLength(0);
        int length2 = objNames.GetLength(1); 
        int totalObjects = objNames.Length;
        sizeX = 2.5f;
        sizeY = totalObjects * 1f + 1;
        float thickness = 0.05f;
        checkLinePos =  new Vector2(posX + (sizeX / 2), posY - ( sizeY / 2));
        checkLine.transform.position = checkLinePos;
        checkLine.transform.localScale = new Vector2(thickness, sizeY);
        Vector2[] frameSet =
        {
            //フレーム位置
            new Vector2(posX, posY), //up
            new Vector2(posX, posY - sizeY), //down
            new Vector2(posX - (sizeX / 2), posY - ( sizeY / 2)), //left
            new Vector2(posX + (sizeX / 2), posY - ( sizeY / 2)), //right
            //フレームサイズ
            new Vector2(sizeX, thickness), //up
            new Vector2(sizeX, thickness), //down
            new Vector2(thickness, sizeY), //left
            new Vector2(thickness, sizeY) //right
        } ;
        for(int i = 0; i < 4; i++)
        {
            frames[i].transform.position = frameSet[i];
            frames[i].transform.localScale = frameSet[i + 4];
        }
        //オブジェクトを生成し長さを計測
        objForCheckLength = new GameObject[totalObjects];
        for(int j = 0; j < length1; j++)
        {
            for(int k = 0; k < length2; k++)
            {
                Vector2 generatePos = new Vector2( posX, (posY - (j * 4) - (k + 1)));
                GameObject testPrefab = (GameObject)Resources.Load(objNames[j , k]);
                objForCheckLength[(j * 4) + k] = Instantiate(testPrefab, generatePos, Quaternion.identity);
                if(j == 1)
                {
                    Transform wallTransform = objForCheckLength[(j * 4) + k].GetComponent<Transform>();
                    wallTransform.Rotate(0f, 0f, 90f); 
                    
                }
            }
        }
        
        playerYPrev = player.transform.position.y;
        startHeight = (int)player.transform.position.y;
        tileY = 0;
        currentObj = 0;
        objectType[0] = 0;
        startCount = 0;
        count = 0;
        lastEItemY = 0;
        target = 0;
        slimeCount = 0;
        batCount = 0;
        deadLine = -7;
        difficulty = 0;
        for (int i = 0; i < 20; i++)
        {
            objActive[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        currentHeight = (int)player.transform.position.y;
        playerHeight = currentHeight - startHeight;
        heightText.text = (currentHeight - startHeight).ToString() + "M";

        //長さを計測
        if (TimeScript.startTime > 0)
        {
            checkLinePos.x -= 1.0f * Time.deltaTime;
            checkLine.transform.position = checkLinePos;
            int length1 = objNames.GetLength(0);
            int length2 = objNames.GetLength(1); 
            if(TimeScript.startTime < 0.5f && startCount == 0)
            {
                for(int i = 0; i < length1; i++)
                {
                    for(int j = 0; j < length2; j++)
                    {
                        //衝突位置をもとにオブジェクトの長さを求める
                        float distansFromRightFrame = (posX + (sizeX / 2)) - collisionPos[(i * 4) + j]; 
                        eachLength[i, j] = sizeX - (distansFromRightFrame * 2);        
                        Destroy(objForCheckLength[(i * 4) + j]);
                    }
                }
                startCount++;
            }
        }
        else
        {
            DeleteObject();
            SetTiles();
            SetNumbers();
            if (objActive[currentObj] == false)
            {
                // 次に生成するオブジェクトの種類を決定
                objectType[currentObj] = Random.Range(0, 2);
                int maxLength = 3;
                if (objectType[currentObj] == Floor)
                {
                    maxLength = 4;
                }
           
                difficulty = 1 + (playerHeight / 100);
                float rndTmp = Random.Range(0, ( (100 / maxLength)  + (difficulty * (maxLength * maxLength) ) ) ) ;
                float[] provbability = new float[maxLength];
                for (int i = 0; i < maxLength; i++)
                {
                    provbability[i] = ((100 / maxLength ) + difficulty * ((i + 1) * (i + 1)));
                    if (rndTmp < provbability[i]) 
                    {
                        objLength = maxLength - i;
                        rndTmp = nullNumber;
                    }
                }
                SetObjectPos(currentObj);
                GenerateObjects(currentObj);
                objActive[currentObj] = true;
                currentObj++;
                if (currentObj == objUnit) { currentObj = 0; }
                count++;
            }
        }
        if((int)TimeScript.pastTime > deleteDuration)
        {
            DeleteTiles(castleTile);
            DeleteTiles(castleBack);
            DeleteTiles(skyTile);
            deleteDuration += 5;
        }

    }
    void GenerateObjects(int targetNum)
    {

        GameObject parentObject = GameObject.Find("GeneratedObjects");
        Transform parentTransform = parentObject.transform;
        GameObject prefabObj = (GameObject)Resources.Load(objNames[objectType[targetNum], objLength - 1]);
        obj[targetNum] = Instantiate(prefabObj, objPos[targetNum], Quaternion.identity);
        string[] objDirectionName = {"Right", "Left"};
        obj[targetNum].name = objNames[objectType[targetNum] , objLength - 1]  + objDirectionName[objDirection] + "-" + targetNum.ToString();
        obj[targetNum].transform.SetParent(parentTransform);
        //イベントアイテム生成
        if (objPos[targetNum].y > lastEItemY + 7 && objectType[targetNum] == Floor)
        {
            Vector2 eItemPos = objPos[targetNum];
            eItemPos.y += 0.2f;
            GameObject eItemObj = (GameObject)Resources.Load("EventItem");
            GameObject eItem = Instantiate(eItemObj, eItemPos , Quaternion.identity);
            eItem.name = "EventItem" + targetNum.ToString();
            eItem.transform.SetParent(parentTransform);
            Debug.Log("dwadadwadad");
            lastEItemY = objPos[targetNum].y;
        }
        //敵オブジェクト生成(スライム)
        if (objectType[targetNum] == Floor && objLength == 4)
        {
            Vector3 enemyPos = objPos[targetNum];
            Vector3 candlePos = enemyPos;
            enemyPos.y += 0.1f;
            candlePos.y += 0.5f;
            GameObject enemyObj = (GameObject)Resources.Load("slime");
            GameObject candleObj = (GameObject)Resources.Load("Candle");
            slimes[slimeCount] = Instantiate(enemyObj, enemyPos, Quaternion.identity);
            GameObject candle = Instantiate(candleObj, candlePos, Quaternion.identity);

            slimes[slimeCount].transform.SetParent(parentTransform);
            candle.transform.SetParent(parentTransform);
            slimeCount++;
            if(slimeCount == slimeMax) 
            {
                slimeCount = 0;
            }
        }
        //敵オブジェクト生成(コウモリ)
        if (targetNum % 10 == 0)
        {
            Vector3 batSpownPos = objPos[targetNum];
            batSpownPos.y += 0.3f;
            GameObject batObj = (GameObject)Resources.Load("Bat");
            bats[batCount] = Instantiate(batObj, batSpownPos, Quaternion.identity);
            bats[batCount].transform.SetParent(parentTransform);
            batCount++;
            if (batCount == batMax)
            {
                batCount = 0;
            }
        }
    }
    void SetObjectPos(int targetNum)
    {
        //一番最初のオブジェクトの設定
        if (count == 0)
        {
            objectType[targetNum] = Floor;　//最初は床オブジェクトを生成
            objPos[targetNum] = new Vector3(1.0f, -3.8f, 0);
            objectType[0] = 0;
            return;
        }

        //次に生成するオブジェクトの方向を決定
        objDirection = Random.Range(0, 2);  //0・・Left  1・・Right 
        //前のオブジェクトが壁の時壁ジャンプの方向にオブジェクトを生成
        if (objectType[prev] == Wall && count > 1)
        {
            if (((objPos[prev].x - objPos[prev2].x) > 0))
            {
                objDirection = Left;
            }
            else { objDirection = Right; }
        }

        while (true) 
        {
            //前のオブジェクトとの距離を設定
            switch (objectType[targetNum])
            {
                case Floor: //床        
                    xMin = 0.5f; xMax = 0.8f;
                    yMin = 0.55f; yMax = 0.60f;
                    //壁→床の時
                    if (count >= 1 && objectType[prev] == Wall)
                    {
                        xMin = 0.6f; xMax = 0.9f;
                        yMin = 0.3f; yMax = 0.4f;
                        yMin -= 0.1f * eachLength[objectType[targetNum], objLength - 1];
                        yMax -= 0.1f * eachLength[objectType[targetNum], objLength - 1];
                    }
                    xMin += 0.3f * eachLength[objectType[targetNum], objLength - 1];
                    xMax += 0.3f * eachLength[objectType[targetNum], objLength - 1];
                    break;
                case Wall:  //壁      
                    xMin = 0.6f; xMax = 0.8f;
                    yMin = 0.4f; yMax = 0.6f;
                    //床→壁の時
                    if (count >= 1 && objectType[prev] == Floor)
                    {
                        xMin = 0.7f; xMax = 0.9f;
                        yMin = 0.7f; yMax = 0.9f;
                    }
                    break;
            }

            // 新しいオブジェクトの位置を計算
            Vector3 newObjPos = new Vector3();
            newObjPos.y = Random.Range(objPos[prev].y + yMin, objPos[prev].y + yMax);
            if (objDirection == Right)
            {
                newObjPos.x = Random.Range(objPos[prev].x + xMin, objPos[prev].x + xMax);
            }
            else
            {
                newObjPos.x = Random.Range(objPos[prev].x - xMin, objPos[prev].x - xMax);
            }

            if (newObjPos.x > rightLimit - (eachLength[objectType[targetNum], objLength - 1] / 2))
            {
                objDirection = Left;
                objectType[targetNum] = Floor;
                if(objectType[prev] == Wall) 
                {
                    objectType[targetNum] = Wall;
                    objDirection = Right;
                }
            }
            else if (newObjPos.x < leftLimit + (eachLength[objectType[targetNum], objLength - 1] / 2))
            {
                objDirection = Right;
                objectType[targetNum] = Floor;
                if (objectType[prev] == Wall)
                {
                    objectType[targetNum] = Wall;
                    objDirection = Left;
                }
            }
            else 
            {
                objPos[targetNum] = newObjPos;
                break; 
            }
        }

    }


    void DeleteObject()
    {
        Vector3 playerPos = player.transform.position;
        if (playerPos.y > 2.5f && playerPos.y - objPos[target].y > 4 && obj[target] != null)
        {
            deadLine = objPos[target].y;
            deadLine -= 2;
            Destroy(obj[target]);
            objActive[target] = false;
            target++;
            if (target >= objUnit)
            {
                target = 0;
            }
        }
    }
    int drowPosY = -2;
   
    void SetTiles()
    {
        Vector3 playerPos = player.transform.position;
        if (playerPos.y - playerYPrev > 0.2f) 
        {
            playerYPrev = playerPos.y;
            drowPosY++;
        }
            for (int x = -7; x < 6; x++)
            {
                castleBack.SetTile(new Vector3Int(x, drowPosY, 0), backBrick);
            }
        castleTile.SetTile(new Vector3Int(-8, drowPosY, 0), sideWall);
        castleTile.SetTile(new Vector3Int( 6, drowPosY, 0), sideWall);
    }

    void SetNumbers()
    {
        prev = currentObj - 1;
        if (prev == -1)
        { 
            prev = objUnit - 1;
        }
        prev2 = prev -1;
        if(prev2 == -1)
        {
            prev2 = objUnit - 1;
        }
    }


    void DeleteTiles(Tilemap tilemap)
    {
        Vector3 playerPos = player.transform.position;
        float maxY = playerPos.y - 7;
        // タイルの位置情報を取得
        BoundsInt bounds = tilemap.cellBounds;

        // Y 座標が閾値未満のタイルを検索して削除
        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
        {
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                Vector3 tileWorldPosition = tilemap.GetCellCenterWorld(cellPosition);

                if (tileWorldPosition.y < maxY)
                {
                    // 条件を満たすタイルを削除
                    tilemap.SetTile(cellPosition, null);
                }
            }
        }
    }
}
