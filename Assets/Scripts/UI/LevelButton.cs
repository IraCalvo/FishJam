using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] int levelIndex;

    public void SetInteracble(bool interactable)
    {
        GetComponent<Button>().interactable = interactable;
    }

    public void ButtonClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        SceneManager.LoadScene(levelIndex);
    }
}
