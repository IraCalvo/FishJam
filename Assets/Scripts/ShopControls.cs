using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ShopControls : MonoBehaviour
{
    public void OnBuyItem(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            switch (callbackContext.control.name)
            {
                case "1":
                    FishSpawner.Instance.SpawnFish(1);
                    break;
                case "2":
                    FishSpawner.Instance.SpawnFish(2);
                    break;
                case "3":
                    FishSpawner.Instance.SpawnFish(3);
                    break;
                case "4":
                    FishSpawner.Instance.SpawnFish(4);
                    break;
                case "5":
                    FishSpawner.Instance.SpawnFish(5);
                    break;
                case "6":
                    FishSpawner.Instance.SpawnFish(6);
                    break;
                case "7":
                    FishSpawner.Instance.SpawnFish(7);
                    break;
                default:
                    break;
            }
        }
    }

    public void OnClickItem(BaseEventData baseEventData)
    {
        FishSpawner.Instance.SpawnFish(1);
    }
}
