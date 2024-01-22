using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiotiming : MonoBehaviour
{
    public AudioSource backgroundMusicAudioSource;
    public AudioSource victoryAudioSource;


    public void OnLevelComplete()
    {
        Debug.Log("Muted");
        backgroundMusicAudioSource.volume = 0;
        victoryAudioSource.Play();

    }
}
