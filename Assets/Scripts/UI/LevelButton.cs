using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] GameObject levelMenu;
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
            levelMenu.transform.position = new Vector3(660, levelMenu.transform.position.y, 0);
            levelDescriptionBox.SetActive(true);
            levelDescriptionBoxText.text = levelDescrition;
            levelDescriptionBoxLevelNumber.text = levelNumberText;
            playButton.GetComponent<PlayLevelButton>().levelToLoad = levelNumber;
            
        }
        else if (levelDescriptionBox.activeInHierarchy && playButton.GetComponent<PlayLevelButton>().levelToLoad == levelNumber)
        {
            levelDescriptionBox.SetActive(false);
            levelMenu.transform.position = new Vector3(0, levelMenu.transform.position.y, 0);
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
