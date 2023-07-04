using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Collision : MonoBehaviour
{
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("Wall"))
            {
                if(count == 0)
                { 
                    this.gameObject.GetComponent<Light2D>().intensity = 0;
                    SoundEffect.sound3Trigger = true;
                    GameSystem.combo++;
                    GameSystem.score += 15 * GameSystem.combo / 5;
                    TimeScript.elapsedTime += 1;
                    count++;
                }
                else if (count == 1)
                {
                    SoundEffect.sound4Trigger = true;
                    GameSystem.combo = 0;
                }

            }
           
            if (this.gameObject.CompareTag("Surface"))
            {
                if(count == 0)
                {  
                    this.gameObject.GetComponent<Light2D>().intensity = 0;
                    SoundEffect.sound3Trigger = true;
                    GameSystem.combo++;
                    GameSystem.score += 5 * GameSystem.combo / 5;
                    TimeScript.elapsedTime += 1;
                    count++;
                }
                else if(count == 1)
                {
                    SoundEffect.sound4Trigger = true;
                    GameSystem.combo = 0;
                }
            }
            if (this.gameObject.CompareTag("Enemy"))
            {
               GameSystem.score -= 5;
               GameSystem.combo = 0;
               SoundEffect.sound1Trigger = true;
               Debug.Log(other.gameObject.name + "と衝突しました");
               Destroy(this.gameObject);
            }   
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(this.gameObject.name.Contains("Check"))
        {
            int objNumber = 0;
            if(other.gameObject.name.Contains("Wall"))
            {
                objNumber += 4;
            }
            for(int i = 0; i < 4; i++)
            {
                if(other.gameObject.name.Contains((i + 1).ToString()))
                {
                    objNumber += i;
                }
            }
            GenerateStage.collisionPos[objNumber] = this.gameObject.transform.position.x;
        }
        
    }
}

