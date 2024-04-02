using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    void OnFire(InputValue value)
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
}