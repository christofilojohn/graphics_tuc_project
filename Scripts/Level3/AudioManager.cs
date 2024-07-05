using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3.scripts
{
    
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip ouchSound;
    [SerializeField] private AudioSource sfxSpeaker;

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
    public void PlayOuch()
    {
        PlaySound(ouchSound);
    }
}


}