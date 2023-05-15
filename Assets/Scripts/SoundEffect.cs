using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip Dyukushi,Bun, Kirarin;
    public static bool DyukushiTrigger, BunTrigger, KirarinTrigger;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DyukushiTrigger)
        {
            audioSource.PlayOneShot(Dyukushi);
            DyukushiTrigger = false;
        }
        if (BunTrigger)
        {
            audioSource.PlayOneShot(Bun);
            BunTrigger = false;
        }
        if (KirarinTrigger)
        {
            audioSource.PlayOneShot(Kirarin);
            KirarinTrigger = false;
        }
    }

}
