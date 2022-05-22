using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    AudioSource soundSystem;
    
    void Awake()
    {
        soundSystem = this.GetComponent<AudioSource>();
        if (soundSystem == null){
            Debug.Log("There must be an AudioSource attached to this object!");
        }
    }

    // Update is called once per frame
    public void PlaySound(AudioClip clip, float volumeScale = 1.0f){
        soundSystem.PlayOneShot(clip, volumeScale);
    }
}
