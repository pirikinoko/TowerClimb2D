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
            if(count == 0)
            {
              
                if (this.gameObject.CompareTag("Enemy"))
                {
                    GameSystem.score -= 5;
                    GameSystem.combo = 0;
                    SoundEffect.DyukushiTrigger = true;
                    Destroy(this.gameObject);
                }   

                if (this.gameObject.CompareTag("Surface"))
                {
                    this.gameObject.GetComponent<Light2D>().intensity = 0;
                    SoundEffect.KirarinTrigger = true;
                    GameSystem.combo++;
                    GameSystem.score += 5 * GameSystem.combo / 5;
                    TimeScript.elapsedTime += 1;
                    count++;
                }
                if (this.gameObject.CompareTag("Wall"))
                {
                    this.gameObject.GetComponent<Light2D>().intensity = 0;
                    SoundEffect.KirarinTrigger = true;
                    GameSystem.combo++;
                    GameSystem.score += 15 * GameSystem.combo / 5;
                    TimeScript.elapsedTime += 1;
                    count++;
                }
            }
          
        }

    }

}
