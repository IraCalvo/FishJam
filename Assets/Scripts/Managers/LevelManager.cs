using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelButton[] levelButton;

    private void Start()
    {
        if (PlayerPrefs.HasKey("HighestLevel") == false)
        {
            PlayerPrefs.SetInt("HighestLevel", 1);
        }

        int highestLevel = PlayerPrefs.GetInt("HighestLevel", 0);
        for (int i = 0; i < levelButton.Length; i++)
        {
            levelButton[i].SetInteracble(i <= highestLevel);
        }
    }
}
