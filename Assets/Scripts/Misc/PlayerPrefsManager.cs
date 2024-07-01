using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    void Awake()
    {
        if (PlayerPrefs.HasKey("Sand Dollar") == false)
        {
            PlayerPrefs.SetInt("Sand Dollar", 0);
        }

        if (PlayerPrefs.HasKey("Angler Fish") == false)
        {
            PlayerPrefs.SetInt("Angler Fish", 0);
        }

        if (PlayerPrefs.HasKey("Music Volume") == false)
        { 
            PlayerPrefs.SetFloat("Music Volume", 1f);
        }

        if (PlayerPrefs.HasKey("SFX Volume") == false)
        {
            PlayerPrefs.SetFloat("SFX Volume", 1f);
        }

        if (PlayerPrefs.HasKey("Music Volume Before Muted") == false)
        {
            PlayerPrefs.SetFloat("Music Volume Before Muted", 1f);
        }

        if (PlayerPrefs.HasKey("SFX Volume Before Muted") == false)
        {
            PlayerPrefs.SetFloat("SFX Volume Before Muted", 1f);
        }
    }
}
