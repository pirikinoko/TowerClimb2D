using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GenerateStage : MonoBehaviour
{
    [SerializeField] Tilemap castleBack, castleTile, skyTile;
    [SerializeField] Tile backBrick, sideWall;
    [SerializeField] GameObject player;
    const int Floor = 0, Wall = 1, Right = 0, Left = 1, objUnit = 30;
    const float rightLimit = 1.9f, leftLimit = -2.1f;
    GameObject[] obj = new GameObject[objUnit];
    bool[] objActive = new bool[objUnit];
    Vector3[] objPos = new Vector3[objUnit];
    public float deadLine { get; set; }
    int[] objectType = new int[objUnit];
    float xMax = 0, xMin = 0,  yMax = 0, yMin = 0, playerYPrev;
    int currentObj = 0, count = 0, target = 0, tileY, objLength, objDirection = 0;
    string [,] objNames = { { "Floor1", "Floor2", "Floor3" }, { "Wall1", "Wall2", "Wall3" } };
    // Start is called before the first frame update
    void Start()
    {
        playerYPrev = player.transform.position.y;
        tileY = 0;
        currentObj = 0;
        objectType[0] = 0;
        count = 0;
        target = 0; 
        deadLine = -10;
        for (int i = 0; i < 20; i++)
        {
            objActive[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DeleteObject();
        SetTiles();
        if (objActive[currentObj] == false)
        {
            // 次に生成するオブジェクトの種類を決定
            objectType[currentObj] = Random.Range(0, 2);
            int maxLength = 3;
            if (objectType[currentObj] == Wall)
            {
                maxLength = 3;
            }
            objLength = Random.Range(1, maxLength + 1);
            SetObjectPos(currentObj);
            GenerateObjects(currentObj);
            objActive[currentObj] = true;
            currentObj++;
            if (currentObj == objUnit) { currentObj = 0; }
            count++;
        }
    }
    void GenerateObjects(int targetNum)
    {
        GameObject prefabObj = (GameObject)Resources.Load(objNames[objectType[targetNum], objLength - 1]);
        obj[targetNum] = Instantiate(prefabObj, objPos[targetNum], Quaternion.identity);
        obj[targetNum].name = objNames[objectType[targetNum] , objLength - 1] + "-" + targetNum.ToString(); 
    }
    void SetObjectPos(int targetNum)
    {
        if(count == 0) 
        {
            objPos[targetNum] = new Vector3(-1.5f, -3.8f, 0);
            return;
        }
        int prev = targetNum - 1;
        if (prev == -1) { 
            prev = objUnit - 1;
        }

        if (count == 0) { objectType[targetNum] = 0; }

        switch(objectType[targetNum])
        {
            case Floor:　//床        
            xMin = 0.5f; xMax = 0.8f;
            yMin = 0.4f; yMax = 0.6f;
            //壁→床の時
            if (count >= 1 && objectType[prev] == Wall)
            {
                xMin = 0.6f; xMax = 0.9f;
                yMin = 0.2f; yMax = 0.3f;
            }
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

        if (targetNum == 0)
        {
            prev = objUnit - 1;
        }
        //次に生成するオブジェクトの方向を決定
        objDirection = Random.Range(0, 2); // 0か1が出る
        int prev2 = prev -1;
        if(prev2 == -1)
        {
            prev2 = objUnit - 1;
        }
     
        if(count > 1 && leftLimit < objPos[prev].x && objPos[prev].x < leftLimit + 0.2) //ステージ範囲に収める
        {
            objDirection = Right;
        }
        else if ( count > 1 && rightLimit - 0.2f < objPos[prev].x && objPos[prev].x < rightLimit)
        {
            objDirection = Left;
        }
        if (objectType[prev] == Wall && count > 1)
        {
            if (((objPos[prev].x - objPos[prev2].x) > 0))
            {
                objDirection = Left;
            }
            else { objDirection = Right; }
        }
        Debug.Log("count" + count + "objDirection" + objDirection);

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

        newObjPos.x = Mathf.Clamp(newObjPos.x, leftLimit, rightLimit);
        objPos[targetNum] = newObjPos;
    }


    void DeleteObject()
    {
        Vector3 playerPos = player.transform.position;
        if (playerPos.y - objPos[target].y > 4 && obj[target] != null)
        {
            deadLine = objPos[target].y;
            deadLine -= 3;
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

}
