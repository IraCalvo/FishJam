using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public string nextLevelName;

    public void ButtonPressed()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}
