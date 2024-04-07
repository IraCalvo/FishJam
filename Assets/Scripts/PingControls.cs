using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class PingControls : MonoBehaviour
{
    public void OnMoveFish(CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Vector2 centerMovePosition = Camera.main.ScreenToWorldPoint(mousePos);

            foreach (Fish fish in BoxSelection.Instance.selectedFish)
            {
                if (fish != null)
                {
                    if (fish.TryGetComponent<FishMovement>(out FishMovement fishMovement1))
                    {
                        FishMovement fishMovement = fish.GetComponent<FishMovement>();
                        fishMovement.targetPosition = centerMovePosition;
                    }
                    else if (fish.TryGetComponent<CrabMovement>(out CrabMovement crabMovement1))
                    {

                    }
                }
                
            }
        }
    }
}