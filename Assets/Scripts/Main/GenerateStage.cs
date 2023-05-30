using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStage : MonoBehaviour
{
    GameObject[] obj = new GameObject[20];
    bool[] objActive = new bool[20];
    Vector3[] objPos = new Vector3[20];
    public float xMax, xMin, yMax, yMin;
    int objType = 0, currentObj = 0, count = 0;
    string [] objNames = { "Floor1" , "Wall1"};
    const float rightLimit =  1.8f, leftLimit = -1.8f;
    // Start is called before the first frame update
    void Start()
    {
        currentObj = 0;
        objType = 0;
        count = 0;
        for (int i = 0; i < 20; i++)
        {
            objActive[i] = false;
        }
        objPos[19] = new Vector3(0, -4.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {   
        if(objActive[currentObj] == false)
        {
            SetObjectPos(currentObj);
            GenerateObjects(currentObj);
            objActive[currentObj] = true;
            currentObj++;       
            count++;

            if(currentObj == 20) { currentObj = 0; }
        }
    }
    void GenerateObjects(int targetNum)
    {
        obj[targetNum] = (GameObject)Resources.Load(objNames[objType]);
        Instantiate(obj[targetNum], objPos[targetNum], Quaternion.identity);
        obj[targetNum].name = objType + targetNum.ToString();
    }
    void SetObjectPos(int targetNum)
    {
        int prev = targetNum - 1;
        float rateX = 1f, rateY = 1f;

        // 次に生成するオブジェクトの種類を決定
        objType = Random.Range(0, objNames.Length);
        if (count == 0) { objType = 0; }

        if (objType == 1)
        {
            rateX = 0.6f;
            rateY = 1.2f;
        }
        if (count > 2 && objType == 0 && obj[prev].name.Contains("Wall"))
        {
            rateY = 0.5f;
        }
        if (targetNum == 0)
        {
            prev = 19;
        }

        int objDirection = Random.Range(0, 2); // 0か1が出る
        if (count > 2 && ((objPos[prev].x - objPos[prev - 1].x) > 0))
        {
            objDirection = 0;
        }
        else if (count > 2 && ((objPos[prev].x - objPos[prev - 1].x) < 0))
        {
            objDirection = 1;
        }
        if(count > 2 && obj[prev].name.Contains("Floor") && leftLimit < objPos[prev].x && objPos[prev].x < leftLimit + 0.2)
        {
            objDirection = 1;
        }
        else if ( count > 2 && obj[prev].name.Contains("Floor") && rightLimit - 0.2f < objPos[prev].x && objPos[prev].x < rightLimit)
        {
            objDirection = 0;
        }


        // 新しいオブジェクトの位置を計算
        Vector3 newObjPos = new Vector3();
        newObjPos.y = Random.Range(objPos[prev].y + yMin * rateY, objPos[prev].y + yMax * rateY);
        if (objDirection == 1)
        {
            newObjPos.x = Random.Range(objPos[prev].x + xMin * rateX, objPos[prev].x + xMax * rateX);
        }
        else 
        {
            newObjPos.x = Random.Range(objPos[prev].x - xMin * rateX, objPos[prev].x - xMax * rateX);
        }

     
      

        newObjPos.x = Mathf.Clamp(newObjPos.x, leftLimit, rightLimit);
        objPos[targetNum] = newObjPos;
    }


}
