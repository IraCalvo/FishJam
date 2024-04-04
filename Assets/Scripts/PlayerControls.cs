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

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Resource"))
            {
                Resource resourceScript = hit.collider.GetComponent<Resource>();

                if (resourceScript != null)
                {
                    resourceScript.ResourceClicked();
                }
            }
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
}
