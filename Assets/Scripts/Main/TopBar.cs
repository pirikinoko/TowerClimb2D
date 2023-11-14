using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    const int Iconval = 3;
    GameObject[] iconObj = new GameObject[Iconval];
    Vector2 iconStartPos, iconEndPos;
    const float generateDuration = 15, maxEnemy = 20;
    float generateTimer, speed = 0.05f;
    string[] icons = { "BuffIcon", "SlimeIcon", "Pixel3" };
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
            int rnd = Random.Range(0, Iconval);
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

        for (int i = 0; i < Iconval; i++)
        {
            if (iconObj[i] != null)
            {
                Vector2 iconPos = iconObj[i].transform.position;
                iconPos.x -= speed * Time.deltaTime;
                iconObj[i].transform.position = iconPos;
                iconEndPos = GameObject.Find("EndLine").transform.position;
                if (iconObj[i].transform.position.x < iconEndPos.x)
                {
                  if(i == 0) 
                    {
                        Effect1();
                    }
                    if (i == 1)
                    {
                        Effect2();
                    }
                    if (i == 2)
                    {
                        Effect3();
                    }
                }
            }
        }
    }

    void Effect1() 
    {
        Destroy(iconObj[0]);
        BuffManagement.buffTrigger[0] = true;
        SoundEffect.sound5Trigger = true;
    }
    void Effect2()
    {
        Destroy(iconObj[1]);
        StartCoroutine(GenerateSlimes());
      
        SoundEffect.sound5Trigger = true;
    }
    void Effect3()
    {
        Destroy(iconObj[2]);
        SoundEffect.sound5Trigger = true;
    }

    IEnumerator GenerateSlimes()
    {
        GameObject[] enemies = new GameObject[20];
        GameObject enemyObj = (GameObject)Resources.Load("slime2");
        for (int i = 0; i < maxEnemy; i++)
        {
            Vector2 generatePos = GameObject.Find("Player").transform.position;
            generatePos.y += 5;
            enemies[i] = Instantiate(enemyObj, generatePos, Quaternion.identity);
            int rnd1 = Random.Range(-2, 2);
            int rnd2 = Random.Range(-2, 2);
            enemies[i].GetComponent<Rigidbody2D>().velocity =  new Vector2(rnd1, rnd2);
            yield return new WaitForSeconds(0.4f);
        }

    }
}
