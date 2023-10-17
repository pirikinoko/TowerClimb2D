using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManagement : MonoBehaviour
{
    public Sprite speedBuffImg;
    public Image[] buffImg;
    public static float[] buffTime = new float[1];
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
        
    }
}
