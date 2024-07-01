using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class PingControls : MonoBehaviour
{

    Bounds tankBounds;
    private void Awake()
    {
        GameObject tank = GameObject.Find("Tank");
        tankBounds = tank.GetComponent<PolygonCollider2D>().bounds;
    }
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
                        Vector2 randomPosition = centerMovePosition + new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                        randomPosition = new Vector2(Mathf.Clamp(randomPosition.x, tankBounds.min.x, tankBounds.max.x), Mathf.Clamp(randomPosition.y, tankBounds.min.y, tankBounds.max.y));
                        fishMovement.targetPosition = randomPosition;
                        fishMovement.nextLocationTimer = 0;
                    }
                    else if (fish.TryGetComponent<CrabMovement>(out CrabMovement crabMovement1))
                    {

                    }
                }
                
            }
        }
    }
}
