using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [SerializeField] Button musicButton;
    [SerializeField] Button sfxButton;

    [SerializeField] Sprite maxVolumeSFX;
    [SerializeField] Sprite midVolumeSFX;
    [SerializeField] Sprite lowVolumeSFX;
    [SerializeField] Sprite mutedSFX;

    [SerializeField] Sprite musicIcon;
    [SerializeField] Sprite mutedMusicIcon;

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("Music Volume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX Volume", 1f);
        SetMusicButtonSprite(musicSlider.value);
        SetSFXButtonSprite(sfxSlider.value);
    }

    public void ToggleMusic()
    { 
        SFXManager.instance.ToggleMusic();

        if (musicSlider.value == 0)
        {
            musicSlider.value = PlayerPrefs.GetFloat("Music Volume Before Muted", 1f);
        }
        else if(musicSlider.value > 0)
        {
            musicSlider.value = PlayerPrefs.GetFloat("Music Volume", 1f);
        }
        
        SetMusicButtonSprite(musicSlider.value);

    }

    public void ToggleSFX()
    {
        SFXManager.instance.ToggleSFX();

        if (sfxSlider.value == 0)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFX Volume Before Muted", 1f);
        }
        else if (sfxSlider.value > 0)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFX Volume", 1f);
        }

        SetSFXButtonSprite(sfxSlider.value);
    }

    public void MusicVolume()
    {
        SFXManager.instance.MusicVolume(musicSlider.value);
        PlayerPrefs.SetFloat("Music Volume", musicSlider.value);

        if (musicSlider.value != 0)
        {
            PlayerPrefs.SetFloat("Music Volume Before Muted", musicSlider.value);
        }

        SetMusicButtonSprite(musicSlider.value);
    }

    public void SFXVolume()
    { 
        SFXManager.instance.SFXVolume(sfxSlider.value);
        PlayerPrefs.SetFloat("SFX Volume", sfxSlider.value);

        if (sfxSlider.value != 0)
        {
            PlayerPrefs.SetFloat("SFX Volume Before Muted", sfxSlider.value);
        }

        SetSFXButtonSprite(sfxSlider.value);
    }

    void SetMusicButtonSprite(float value)
    {
        if (value > 0)
        {
            musicButton.GetComponent<Image>().sprite = musicIcon;
        }
        else if (value == 0)
        {
            musicButton.GetComponent<Image>().sprite = mutedMusicIcon;
        }
    }

    void SetSFXButtonSprite(float value)
    {
        if (value == 0)
        {
            sfxButton.GetComponent<Image>().sprite = mutedSFX;
        }
        else if (value <= .33f)
        {
            sfxButton.GetComponent<Image>().sprite = lowVolumeSFX;
        }
        else if (value <= .67f)
        {
            sfxButton.GetComponent<Image>().sprite = midVolumeSFX;
        }
        else if (value <= 1f)
        {
            sfxButton.GetComponent<Image>().sprite = maxVolumeSFX;
        }
    }
}
