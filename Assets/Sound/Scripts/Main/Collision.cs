﻿using System.Collections;
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
                    SoundEffect.sound1Trigger = true;
                    Destroy(this.gameObject);
                }   

                if (this.gameObject.CompareTag("Surface"))
                {
                    this.gameObject.GetComponent<Light2D>().intensity = 0;
                    SoundEffect.sound3Trigger = true;
                    GameSystem.combo++;
                    GameSystem.score += 5 * GameSystem.combo / 5;
                    TimeScript.elapsedTime += 1;
                    count++;
                }
                if (this.gameObject.CompareTag("Wall"))
                {
                    this.gameObject.GetComponent<Light2D>().intensity = 0;
                    SoundEffect.sound3Trigger = true;
                    GameSystem.combo++;
                    GameSystem.score += 15 * GameSystem.combo / 5;
                    TimeScript.elapsedTime += 1;
                    count++;
                }
            }
            else if (count == 1)
            {
                if (this.gameObject.CompareTag("Wall") || this.gameObject.CompareTag("Surface"))
                {
                    SoundEffect.sound4Trigger = true;
                    GameSystem.combo = 0;
                }
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