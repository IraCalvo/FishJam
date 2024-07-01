using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject confirmationExitPU;
    public GameObject confirmationRestartPU;
    public GameObject keyBindingsMenu;
    public GameObject audioSettingMenu;
    public GameObject graphicsSettingsMenu;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        confirmationExitPU.SetActive(false);
    }

    public void OnPause()
    {
        if (pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else 
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void OnResumeClicked()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnSettingsClicked()
    {
        settingsMenu.SetActive(true);
    }

    public void OnExitClicked()
    { 
        confirmationExitPU.SetActive(true);
    }

    public void OnYesExitClicked()
    {
        //TODO: add way to 'save' the state of the tank so they can return to it
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void OnNoExitClicked()
    {
        confirmationExitPU.SetActive(false);
    }

    public void OnRestartClicked()
    {
        confirmationRestartPU.SetActive(true);
    }

    public void OnRestartPUYesClicked()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void OnRestartPUNoClicked()
    {
        confirmationRestartPU.SetActive(false);
    }

    public void OnKeyBindingsClicked()
    {
        keyBindingsMenu.SetActive(true);
        audioSettingMenu.SetActive(false);
        graphicsSettingsMenu.SetActive(false);
    }

    public void OnAudioSettingClicked()
    {
        audioSettingMenu.SetActive(true);
        keyBindingsMenu.SetActive(false);
        graphicsSettingsMenu.SetActive(false);
    }

    public void OnGraphicsSettingClicked()
    {
        graphicsSettingsMenu.SetActive(true);
        keyBindingsMenu.SetActive(false);
        audioSettingMenu.SetActive(false);
    }


}
