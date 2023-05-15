using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;
        //カメラとプレイヤーの位置を同じにする(追従)
        transform.position = new Vector3(playerPos.x, playerPos.y, -10);
    }
}
