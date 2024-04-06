using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ShopControls : MonoBehaviour
{

    public ShopUIManager shopUIManager;
    private List<ShopItem> shopItems;

    private void Awake()
    {
        
    }

    public void OnBuyItem(InputAction.CallbackContext callbackContext)
    {
        CreateShopItemList();
        int number;
        if (int.TryParse(callbackContext.control.name, out number)) {
            if (callbackContext.performed)
            {
                FishSpawner.Instance.SpawnFish(shopItems[number-1].shopGameObject);
            }
        }
    }

    private void CreateShopItemList()
    {
        List<ShopItem> temp = new List<ShopItem>();
        foreach (GameObject gameObject in shopUIManager.shopItems)
        {
            if (gameObject.TryGetComponent<ShopItem>(out ShopItem shopItem)) {
                temp.Add(shopItem);
            }
        }
        shopItems = temp;
    }

    public void OnClickItem(BaseEventData baseEventData)
    {
        if (TryGetComponent<ShopItem>(out ShopItem shopItem))
        {
            FishSpawner.Instance.SpawnFish(shopItem.shopGameObject);
        }
    }
}
