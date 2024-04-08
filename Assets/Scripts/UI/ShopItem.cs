using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    [SerializeField] public GameObject shopGameObject;

    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI price;

    private void Start()
    {
        LayoutUI();
    }
    public void LayoutUI()
    {
        image.sprite = shopGameObject.GetComponent<SpriteRenderer>().sprite;

        if (shopGameObject.TryGetComponent<Fish>(out Fish fish))
        {
            price.text = "<sprite index=0> " + fish.fishSO.price.ToString();
            if (fish.fishSpecies == FishSpecies.Crab)
            {
                image.rectTransform.sizeDelta = new Vector2(32, 32);
            }
        }
        else if (shopGameObject.TryGetComponent<Trophy>(out Trophy trophy))
        {
            price.text = "<sprite index=0> " + trophy.price;
        }
    }

}
