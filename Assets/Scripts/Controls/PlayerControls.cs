using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerControls : MonoBehaviour
{
    public void OnFire(CallbackContext callbackContext)
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
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Detected enemy");
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    EnemyHealthBar.instance.ShowHealthBar(enemy);
                }
            }
            else
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

    public void OnUseItem(CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Vector2 spawnPoint = Camera.main.ScreenToWorldPoint(mousePos);

            Item item = PlayerManager.Instance.GetCurrentItem();
            item.UseItem(spawnPoint);
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
}
