using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    public void EndDrag(BaseEventData eventData)
    {
        GameObject overlappedGameObject = ShopUIManager.Instance.CheckOverlap((RectTransform)transform);
        if (overlappedGameObject != null)
        {
            ShopUIManager.Instance.SwapItems(gameObject, overlappedGameObject);
        }
        else
        {
            ShopUIManager.Instance.LayoutShopItems();
        }
    }

    public void DragHandler(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, 
            pointerEventData.position, 
            canvas.worldCamera, 
            out position);
        transform.position = canvas.transform.TransformPoint(position);
    }
}
