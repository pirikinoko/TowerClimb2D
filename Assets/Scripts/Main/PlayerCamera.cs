using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    GenerateStage generateStage;

    void Start() 
    {
        generateStage = GameObject.Find("GameSystem").GetComponent<GenerateStage>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;
        if(playerPos.y > generateStage.deadLine + 3) 
        {
            //カメラとプレイヤーの位置を同じにする(追従)
            transform.position = new Vector3(playerPos.x, playerPos.y, -10);
        }
      
    }
}
