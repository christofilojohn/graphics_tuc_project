using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioSource sfxSpeaker;


    private void Awake()
    {
        // Singleton pattern
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        }
        //else
        //{
        //    Destroy(gameObject);
        //}
    //}

    public void PlaySound(AudioClip clip)
    {
        // Play sound from the general sfx speaker
        sfxSpeaker.PlayOneShot(clip);
    }

    public void PlayErrorSound()
    {
        PlaySound(errorSound);
    }
    public void PlaySuccessSound()
    {
        PlaySound(successSound);
    }
    
}

