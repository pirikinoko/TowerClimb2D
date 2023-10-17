using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManagement : MonoBehaviour
{
    public static bool[] buffTrigger = new bool[1];
    public Sprite speedBuffImg;
    public Image[] buffImg;
    float[] buffTime = new float[1];
    float[] setTime = { 5.0f };
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buffTime.Length; i++)
        {
            buffTime[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < buffTime.Length; i++)
        {
            if (buffTrigger[i]) 
            {
                buffTime[i] -= Time.deltaTime;
                if(buffTime[i] < 0) 
                {
                    buffTrigger[i] = false;
                    buffTime[i] = setTime[i];
                }
            }
        }
    }
}
