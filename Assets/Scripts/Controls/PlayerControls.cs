using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerControls : MonoBehaviour
{
    PauseMenuManager pauseMenu;

    private void Start()
    {
        pauseMenu = PauseMenuManager.instance;
    }

    public void OnFire(CallbackContext callbackContext)
    {
        if (PauseMenuManager.instance.pauseMenu.activeSelf == false)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);

            // Variable to keep track of the topmost resource hit
            Resource topmostResource = null;

            // Iterate through all hits to find the topmost resource
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.CompareTag("Resource"))
                {
                    Resource resourceScript = hit.collider.GetComponent<Resource>();

                    // Check if this resource is on top of previous resources
                    if (topmostResource == null || resourceScript.transform.position.z > topmostResource.transform.position.z)
                    {
                        topmostResource = resourceScript;
                    }
                }

                if (hit.collider.TryGetComponent<IClickable>(out var clickable))
                {
                    IClickable clickableObject = clickable;
                    clickableObject.OnClick();
                }

                if (!hit.collider.CompareTag("Enemy"))
                {
                    EnemyHealthBar.instance.healthBarIsActive = false;
                    EnemyHealthBar.instance.gameObject.SetActive(false);
                }
            }

            // If a resource was found, perform the action on the topmost resource
            if (topmostResource != null)
            {
                topmostResource.ResourceClicked();
            }
        }

    }

    public void OnUseItem(CallbackContext callbackContext)
    {
        if (PauseMenuManager.instance.pauseMenu.activeSelf == false)
        {
            if (callbackContext.performed)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();

                Vector2 spawnPoint = Camera.main.ScreenToWorldPoint(mousePos);

                Item item = PlayerManager.Instance.GetCurrentItem();
                item.UseItem(spawnPoint);
            }
        }
    }

    public void OnFishTab(CallbackContext callbackContext)
    {
        Debug.Log("Detected");
        if (FishList.instance.fishListIsActive == false)
        {
            FishList.instance.ShowFishList();
        }
        else
        {
            FishList.instance.RemoveFishList();
        }
    }

    //need to think about if GameManager will exist outisde of game and be there invisible in the main menu
    public void OnEsc(CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (pauseMenu.confirmationExitPU.activeSelf)
            {
                pauseMenu.confirmationExitPU.SetActive(false);
                return;
            }
            else if (pauseMenu.confirmationRestartPU.activeSelf)
            {
                pauseMenu.confirmationRestartPU.SetActive(false);
                return;
            }
            else if (pauseMenu.settingsMenu.activeSelf)
            {
                pauseMenu.settingsMenu.SetActive(false);
                return;
            }
            else if (pauseMenu != null)
            {
                pauseMenu.OnPause();
            }

        }
    }

    public void OnSellFish(CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            foreach (Fish fish in BoxSelection.Instance.selectedFish)
            { 
                if(fish != null)
                {
                    if (GameManager.instance.activeFish.Count > 1)
                    {
                        SFXManager.instance.PlaySFX(SoundType.FishSold);
                        BankManager.Instance.AddMoney(fish.fishSO.sellAmount);
                        PoolManager.instance.DeactivateObjectInPool(fish.gameObject);
                    }
                }
            }
        }
    }
}
