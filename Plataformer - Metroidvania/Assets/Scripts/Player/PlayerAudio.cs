using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip coinSound;
    public AudioClip jumpSound;
    public AudioClip hitSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySFX(AudioClip sfx)
    {
        if (!AudioListener.pause) // Toca o som apenas se o �udio n�o estiver pausado
        {
            audioSource.PlayOneShot(sfx);
        }

        //audioSource.PlayOneShot(sfx);
    }
}
