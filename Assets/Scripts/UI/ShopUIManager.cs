using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public static ShopUIManager Instance { get; private set; }
    public List<GameObject> shopItems = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LayoutShopItems();
    }

    public void LayoutShopItems()
    {
        int x = -845;
        foreach (GameObject shopItem in shopItems)
        {
            RectTransform rectTransform = (RectTransform)shopItem.transform;
            ((RectTransform)shopItem.transform).anchoredPosition = new Vector2(x, 464);
            x += 230;
        }
    }

    public GameObject CheckOverlap(RectTransform dragTransform)
    {
        foreach (GameObject shopItem in shopItems)
        {
            RectTransform shopItemTransform = (RectTransform)shopItem.transform;
            // This means it's the same object. Skip it.
            if (dragTransform == shopItemTransform)
            {
                continue;
            }
            float minBounds = shopItemTransform.position.x - 100;
            float maxBounds = shopItemTransform.position.x + 100;
            if (minBounds <= dragTransform.position.x && dragTransform.position.x < maxBounds)
            {
                return shopItem;
            }
        }
        return null;
    }

    public void SwapItems(GameObject draggedObject, GameObject overlappedObject)
    {
        int draggedObjectIndex = shopItems.IndexOf(draggedObject);
        int overlappedObjectIndex = shopItems.IndexOf(overlappedObject);
        if (draggedObjectIndex != -1 &&  overlappedObjectIndex != -1)
        {
            shopItems[draggedObjectIndex] = overlappedObject;
            shopItems[overlappedObjectIndex] = draggedObject;
            LayoutShopItems();
        }
    }
}
