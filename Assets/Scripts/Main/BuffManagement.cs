using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManagement : MonoBehaviour
{
    const int max = 1;
    
    public GameObject[] targetImage;
    public Sprite[] buffIcon;

    public static bool[] buffTrigger = new bool[max];
    int[] callTime = new int[max]; 
    float[] buffTime = new float[max];
    float[] imageAlpha = new float[max];
    Color[] imageColor = new Color[max];
    float[] setTime = { 5.0f , 5.0f};

    int activeBuffNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        activeBuffNumber = 0;
        for (int i = 0; i < max; i++)
        {
            imageAlpha[i] = 0;
            buffTrigger[i] = false;
            imageColor[i] = targetImage[i].GetComponent<Image>().color;
            imageColor[i].a = imageAlpha[i];
            buffTime[i] = setTime[i];
            targetImage[i].GetComponent<Image>().color = imageColor[i];
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

                if (callTime[i] == 0)
                {
                    targetImage[activeBuffNumber].GetComponent<Image>().sprite = buffIcon[i];
                    callTime[i] = 1;
                    imageAlpha[activeBuffNumber] = 1.0f;
                    activeBuffNumber++;
                }

                if (buffTime[i] < 0) 
                {
                    buffTrigger[i] = false;
                    buffTime[i] = setTime[i];
                    callTime[i] = 0;
                    activeBuffNumber--;
                }
            }

            for (int j = 0; j < activeBuffNumber; j++)
            {
                imageAlpha[j] -= Time.deltaTime / setTime[i];
                imageColor[j].a = imageAlpha[j];
                targetImage[j].GetComponent<Image>().color = imageColor[i];
            }
        }
    }
}
