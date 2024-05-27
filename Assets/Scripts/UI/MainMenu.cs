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
        Application.Quit();
    }

    public void InfoButton()
    { 
        infoImage.gameObject.SetActive(true);
    }

    public void ShopButton()
    { 
        shopMenu.gameObject.SetActive(true);
    }

    public void PlayButton()
    { 
        playMenu.SetActive(true);
    }

    public void SurvivaLButton()
    { 
        survivalMenu.SetActive(true);
    }
}
