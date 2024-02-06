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
    float additionalTime = 2;
    //スライム用
    const int maxEnemy = 7;
    float generateTimer, speed = 0.05f;
    int objCount = 0;
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
                Debug.Log("�R���[�`�����I�����܂�"); 
                slotDuration = RslotDuration;
                if(eventNum == 0) { StartCoroutine(GenerateSlimes()); }
                else { BuffManagement.buffTrigger[0] = true; }
                Destroy(GameObject.Find(nameToDestroy));
                break;
            }
        }
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

    void PlusTime()
    {
        TimeScript.elapsedTime += additionalTime;
    }
}
