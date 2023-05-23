using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStage : MonoBehaviour
{
    GameObject[] obj = new GameObject[20];
    bool[] objActive = new bool[20];
    Vector3[] objPos = new Vector3[20];
    public float xMax, xMin, yMax, yMin;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 20; i++)
        {
            objActive[i] = false;
        }
        objPos[19] = new Vector3(0, -4.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {   
        SetObjectPos();
        GenerateObjects();
    }
    void GenerateObjects()
    {
        for(int i = 0; i < 20; i++)
        {
            if(objActive[i] == false)
            {
                obj[i] = (GameObject)Resources.Load("Floor1");
                Instantiate(obj[i], objPos[i], Quaternion.identity);
                objActive[i] = true;
            }
        }
    }
    void SetObjectPos()
    {
        for(int i = 0; i < 20; i++)
        {
            int prev = i - 1;
            if(i == 0){ prev = 19; }
            Vector3 newObjPos = new Vector3(0, 0, 0);
            newObjPos.x = Random.Range(objPos[prev].x + xMin, objPos[prev].x + xMax);
            newObjPos.y = Random.Range(objPos[prev].y + yMin, objPos[prev].y + yMax);
            int rnd = Random.Range(0, 2); //0か1が出る
            if(rnd == 1){ newObjPos.x = Random.Range(objPos[prev].x - xMin, objPos[prev].x - xMax);} 
            newObjPos.x = Mathf.Min(1.8f, newObjPos.x);
            newObjPos.x = Mathf.Max(-1.8f, newObjPos.x);
            
            objPos[i] = newObjPos;
        }
    }


}
