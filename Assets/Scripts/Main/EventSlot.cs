using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSlot : MonoBehaviour
{
    SoundEffect soundEffect;
    public Sprite[] icons;
    Image image;
    public int eventNum = 0;
    float slotDuration = 0.04f, RslotDuration = 0.04f, shrinkSpeed = 1.1f, stopTime = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        soundEffect = GameObject.Find("AudioSystem").GetComponent<SoundEffect>();
        image = GetComponent<Image>();
       // StartCoroutine("RollSlot");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator RollSlot(string nameToDestroy) 
    {
        Debug.Log("SlotDuration== " + slotDuration);
        while (slotDuration < stopTime) 
        {
            eventNum++;
            soundEffect.PlaySE(6);
            if (eventNum == icons.Length)
            {
                eventNum = 0;
            }
            image.sprite = icons[eventNum];
            yield return new WaitForSeconds(slotDuration);
            slotDuration *= shrinkSpeed;
            if(slotDuration > stopTime)
            { 
                Debug.Log("コルーチンを終了します"); 
                slotDuration = RslotDuration;
                Destroy(GameObject.Find(nameToDestroy));
                break;
            }
        }
    }


    //イベント①スライム生成
    int maxEnemy = 7;
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
