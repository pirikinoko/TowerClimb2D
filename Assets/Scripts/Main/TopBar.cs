using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    const int Iconval = 2, maxEnemy = 7;
    GameObject[] iconObj = new GameObject[Iconval];
    Vector2 iconStartPos, iconEndPos;
    const float generateDuration = 15;
    float generateTimer, speed = 0.05f;
    string[] icons = { "BuffIcon", "SlimeIcon", "Pixel3" };
    int objCount = 0;
    int[] eventType = new int[10];
    SoundEffect soundEffect;
    // Start is called before the first frame update
    void Start()
    {
        soundEffect = GameObject.Find("AudioSystem").GetComponent<SoundEffect>();
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
        soundEffect.PlaySE(5);
    }
    void Effect2(int toDestroy)
    {
        Destroy(iconObj[toDestroy]);
        StartCoroutine(GenerateSlimes());

        soundEffect.PlaySE(4);
    }
    void Effect3(int toDestroy)
    {
        Destroy(iconObj[toDestroy]);
        soundEffect.PlaySE(4);
    }

    IEnumerator GenerateSlimes()
    {
        GameObject[] enemies = new GameObject[maxEnemy];
        GameObject enemyObj = (GameObject)Resources.Load("slime");
        for (int i = 0; i < maxEnemy; i++)
        {
            Vector2 generatePos = Vector2.zero;
            generatePos.y = GameObject.Find("Player").transform.position.y;
            generatePos.y += 3;
            enemies[i] = Instantiate(enemyObj, generatePos, Quaternion.identity);
            int rnd1 = Random.Range(-5000, 5000);
            int rnd2 = Random.Range(0, 2000);
            enemies[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(rnd1, rnd2));
            yield return new WaitForSeconds(0.5f);
        }

    }
}
