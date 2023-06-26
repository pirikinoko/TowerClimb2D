using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip sound1, sound2, sound3, sound4;
    public static bool sound1Trigger, sound2Trigger, sound3Trigger, sound4Trigger;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sound1Trigger)
        {
            audioSource.PlayOneShot(sound1);
            sound1Trigger = false;
        }
        if (sound2Trigger)
        {
            audioSource.PlayOneShot(sound2);
            sound2Trigger = false;
        }
        if (sound3Trigger)
        {
            audioSource.PlayOneShot(sound3);
            sound3Trigger = false;
        }
        if (sound4Trigger)
        {
            audioSource.PlayOneShot(sound4);
            sound4Trigger = false;
        }
    }

}
