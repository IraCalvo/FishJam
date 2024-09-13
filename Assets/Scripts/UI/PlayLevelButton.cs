using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayLevelButton : MonoBehaviour
{
    public int levelToLoad;

    public void LevelToLoad()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        SceneManager.LoadScene(levelToLoad);
    }
}
