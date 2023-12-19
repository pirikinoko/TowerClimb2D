using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    const int Iconval = 2;
    GameObject[] iconObj = new GameObject[Iconval];
    Vector2 iconStartPos, iconEndPos;
    const float generateDuration = 15, maxEnemy = 10;
    float generateTimer, speed = 0.05f;
    string[] icons = { "BuffIcon", "SlimeIcon", "Pixel3" };
    int objCount = 0;
    int[] eventType = new int[10];
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
            eventType[objCount] = rnd;
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
                  if(eventType[i] == 0) 
                    {
                        Effect1(i);
                    }
                    if (eventType[i] == 1)
                    {
                        Effect2(i);
                    }
                    if (eventType[i] == 2)
                    {
                        Effect3(i);
                    }
                }
            }
        }
    }

    void Effect1(int toDestroy) 
    {
        Destroy(iconObj[toDestroy]);
        BuffManagement.buffTrigger[0] = true;
        SoundEffect.sound6Trigger = true;
    }
    void Effect2(int toDestroy)
    {
        Destroy(iconObj[toDestroy]);
        StartCoroutine(GenerateSlimes());
      
        SoundEffect.sound5Trigger = true;
    }
    void Effect3(int toDestroy)
    {
        Destroy(iconObj[toDestroy]);
        SoundEffect.sound5Trigger = true;
    }

    IEnumerator GenerateSlimes()
    {
        GameObject[] enemies = new GameObject[10];
        GameObject enemyObj = (GameObject)Resources.Load("slime");
        for (int i = 0; i < maxEnemy; i++)
        {
            Vector2 generatePos = GameObject.Find("Player").transform.position;
            generatePos.y += 3;
            enemies[i] = Instantiate(enemyObj, generatePos, Quaternion.identity);
            int rnd1 = Random.Range(-2, 2);
            int rnd2 = Random.Range(-2, 2);
            enemies[i].GetComponent<Rigidbody2D>().velocity =  new Vector2(rnd1, rnd2);
            yield return new WaitForSeconds(0.4f);
        }

    }
}
