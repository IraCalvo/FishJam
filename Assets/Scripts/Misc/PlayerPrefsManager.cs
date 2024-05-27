using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("Sand Dollar") == false)
        {
            PlayerPrefs.SetInt("Sand Dollar", 0);
        }

        if (PlayerPrefs.HasKey("Angler Fish") == false)
        {
            PlayerPrefs.SetInt("Angler Fish", 0);
        }
    }
}
