﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] SE = new AudioClip[7];
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void PlaySE(int num) 
    {
        if(SE[num] != null && audioSource != null) 
        {
            audioSource.PlayOneShot(SE[num]);
        }
    }
}
