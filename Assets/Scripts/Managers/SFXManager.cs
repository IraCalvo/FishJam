using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public enum SoundType
{ 
    None,
    BGMusic,
    FishEat,
    CoinCollected,
    FishSold,
    SandDollar,
    ButtonPressed,
    NotEnoughMoney,
    Splash
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] Sound[] musicSounds;
    [SerializeField] Sound[] sfxSounds;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    
 
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        float musicVol = PlayerPrefs.GetFloat("Music Volume", 1f);

        if (musicVol == 0)
        {
            musicSource.mute = true;
        }
        else
        {
            musicSource.mute = false;

            float musicBeforeMuted = PlayerPrefs.GetFloat("Music Volume Before Muted", 1f);
            musicSource.volume = musicBeforeMuted;
        }
    }

    private void Start()
    {
        PlayMusic(SoundType.BGMusic);
    }

    public void PlayMusic(SoundType soundToPlay)
    {
        foreach (Sound sounds in musicSounds)
        {
            if (sounds.soundType == soundToPlay)
            {
                int randmizedSound = UnityEngine.Random.Range(0, sounds.clip.Length);
                musicSource.clip = sounds.clip[randmizedSound];
                musicSource.volume = sounds.volume * PlayerPrefs.GetFloat("Music Volume", 1f);
                musicSource.Play();
            }
        }
    }

    public void PlaySFX(SoundType soundToPlay)
    {
        foreach (Sound sounds in sfxSounds)
        {
            if (sounds.soundType == soundToPlay)
            {
                int randomizedSound = UnityEngine.Random.Range(0, sounds.clip.Length);
                sfxSource.PlayOneShot(sounds.clip[randomizedSound], sounds.volume * PlayerPrefs.GetFloat("SFX Volume", 1f));
            }
        }
    }

    public void ToggleMusic()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        if (musicSource.mute == false)
        {
            PlayerPrefs.SetFloat("Music Volume", 0f);

            musicSource.mute = !musicSource.mute;
        }
        else
        {
            float musicBeforeMuted = PlayerPrefs.GetFloat("Music Volume Before Muted", 1f);
            PlayerPrefs.SetFloat("Music Volume", musicBeforeMuted);
            musicSource.mute = !musicSource.mute;
        }
    }

    public void ToggleSFX()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        if (sfxSource.mute == false)
        {
            PlayerPrefs.SetFloat("SFX Volume", 0f);

            sfxSource.mute = !sfxSource.mute;
        }
        else 
        {
            float sfxBeforeMuted = PlayerPrefs.GetFloat("SFX Volume Before Muted", 1f);
            PlayerPrefs.SetFloat("SFX Volume", sfxBeforeMuted);
            sfxSource.mute = !sfxSource.mute;
        }
       
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("Music Volume", volume);
    }

    public void SFXVolume(float volume)
    { 
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFX Volume", volume);
    }
}
