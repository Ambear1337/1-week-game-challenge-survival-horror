using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public PlayerManager player;

    private AudioSource audioSource;

    [SerializeField] private float musicVolumeChangeRate = 0.05f;
    [SerializeField] private float musicMaxVolume = 0.25f;

    public AudioClip calmMusic;
    public AudioClip chaseMusic;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        player = FindObjectOfType<PlayerManager>();
    }

    public void ChangeMusic()
    {
        StartCoroutine(SmoothMusicChange());
    }

    IEnumerator SmoothMusicChange()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.05f;

            yield return new WaitForSeconds(musicVolumeChangeRate);
        }
        
        if (player.PlayerStats.IsChased)
        {
            if (audioSource.clip != chaseMusic)
            {
                audioSource.clip = chaseMusic;
                audioSource.Play();
            }
        }
        
        if (!player.PlayerStats.IsChased)
        {
            if (audioSource.clip != calmMusic)
            {
                audioSource.clip = calmMusic;
                audioSource.Play();
            }
        }
        
        while (audioSource.volume < musicMaxVolume)
        {
            audioSource.volume += 0.05f;

            yield return new WaitForSeconds(musicVolumeChangeRate);
        }
    }
}
