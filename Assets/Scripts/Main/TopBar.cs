using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    GameObject[] iconObj = new GameObject[3];
    Vector2 iconStartPos, iconEndPos;
    const float generateDuration = 15;
    float generateTimer, speed = 0.05f;
    string[] icons = { "BuffIcon", "Pixel2", "Pixel3" };
    int objCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        objCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        generateTimer -= Time.deltaTime;
        if (generateTimer < 0)
        {
            generateTimer = generateDuration;
            int rnd = Random.Range(0, icons.Length);
            GameObject parentObject = GameObject.Find("TopBar");
            GameObject iconPrefab = (GameObject)Resources.Load(icons[rnd]);
            iconObj[objCount] = (GameObject)Instantiate(iconPrefab, parentObject.transform);
            iconObj[objCount].transform.position = GameObject.Find("StartLine").transform.position;
            objCount++;
            if (objCount == iconObj.Length)
            {
                objCount = 0;
            }
        }

        for (int i = 0; i < iconObj.Length; i++)
        {
            if (iconObj[i] != null)
            {
                Vector2 iconPos = iconObj[i].transform.position;
                iconPos.x -= speed * Time.deltaTime;
                iconObj[i].transform.position = iconPos;
                iconEndPos = GameObject.Find("EndLine").transform.position;
                if (iconObj[i].transform.position.x < iconEndPos.x)
                {
                    Destroy(iconObj[i]);
                    BuffManagement.buffTrigger[0] = true;
                }
            }
        }
    }
}
