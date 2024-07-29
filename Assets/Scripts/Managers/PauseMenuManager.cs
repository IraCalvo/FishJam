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
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
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
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnSettingsClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        settingsMenu.SetActive(true);
    }

    public void OnExitClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        confirmationExitPU.SetActive(true);
    }

    public void OnYesExitClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        //TODO: add way to 'save' the state of the tank so they can return to it
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void OnNoExitClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        confirmationExitPU.SetActive(false);
    }

    public void OnRestartClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        confirmationRestartPU.SetActive(true);
    }

    public void OnRestartPUYesClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void OnRestartPUNoClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        confirmationRestartPU.SetActive(false);
    }

    public void OnKeyBindingsClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        keyBindingsMenu.SetActive(true);
        audioSettingMenu.SetActive(false);
        graphicsSettingsMenu.SetActive(false);
    }

    public void OnAudioSettingClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        audioSettingMenu.SetActive(true);
        keyBindingsMenu.SetActive(false);
        graphicsSettingsMenu.SetActive(false);
    }

    public void OnGraphicsSettingClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        graphicsSettingsMenu.SetActive(true);
        keyBindingsMenu.SetActive(false);
        audioSettingMenu.SetActive(false);
    }


}
