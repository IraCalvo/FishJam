using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] GameObject levelDescriptionBox;
    [SerializeField] TextMeshProUGUI levelDescriptionBoxText;
    [SerializeField] TextMeshProUGUI levelDescriptionBoxLevelNumber;
    [SerializeField] Button playButton;
    [TextArea(3, 10)]
    [SerializeField] string levelDescrition;
    [SerializeField] string levelNumberText;
    [SerializeField] int levelNumber;

    public void SetInteracble(bool interactable)
    {
        GetComponent<Button>().interactable = interactable;
    }

    public void ButtonClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        if (!levelDescriptionBox.activeInHierarchy)
        {
            levelDescriptionBox.SetActive(true);
            levelDescriptionBoxText.text = levelDescrition;
            levelDescriptionBoxLevelNumber.text = levelNumberText;
            playButton.GetComponent<PlayLevelButton>().levelToLoad = levelNumber;
        }
        else if (levelDescriptionBox.activeInHierarchy && playButton.GetComponent<PlayLevelButton>().levelToLoad == levelNumber)
        {
            levelDescriptionBox.SetActive(false);
        }

        if (levelDescriptionBox.activeInHierarchy && playButton.GetComponent<PlayLevelButton>().levelToLoad != levelNumber)
        {
            Debug.Log("Called");
            levelDescriptionBoxText.text = levelDescrition;
            levelDescriptionBoxLevelNumber.text = levelNumberText;
            playButton.GetComponent<PlayLevelButton>().levelToLoad = levelNumber;
        }
        //else
        //{
        //    levelDescriptionBox.SetActive(false);
        //}
    }
}
