using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image infoImage;
    public GameObject playMenu;
    public GameObject survivalMenu;
    public GameObject shopMenu;

    private void Start()
    {
        playMenu.SetActive(false);
        //survivalMenu.SetActive(false);
        //shopMenu.SetActive(false);
    }

    public void QuitButton()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        Application.Quit();
    }

    public void InfoButton()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        infoImage.gameObject.SetActive(true);
    }

    public void ShopButton()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        shopMenu.gameObject.SetActive(true);
    }

    public void PlayButton()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        playMenu.SetActive(true);
    }

    public void SurvivaLButton()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        survivalMenu.SetActive(true);
    }
}
