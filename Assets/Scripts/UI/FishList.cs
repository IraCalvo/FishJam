using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class FishList : MonoBehaviour
{
    public static FishList instance;
    public bool fishListIsActive;
    public List<TextMeshProUGUI> fishAmountText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        gameObject.SetActive(false);
        fishListIsActive = false;
    }
     
    public void ShowFishList()
    {
        gameObject.SetActive(true);
        fishListIsActive = true;
        UpdateFishList();
    }

    public void RemoveFishList()
    {
        gameObject.SetActive(false);
        fishListIsActive = false;
    }

    public void UpdateFishList()
    {
        List<GameObject> shopUIManagerShopItems = ShopUIManager.Instance.shopItems;
        for (int i = 0; i < fishAmountText.Count; i++)
        {
            for (int s = 0; s < ShopUIManager.Instance.shopItems.Count; s++)
            {
                ShopItem shopItem = shopUIManagerShopItems[s].GetComponent<ShopItem>();
                GameObject shopItemGameObject = shopItem.shopGameObject;

                //this shop item is a fish
                if (shopItemGameObject.TryGetComponent<Fish>(out Fish fish) != false)
                {
                    fishAmountText[s].gameObject.SetActive(true);
                    int fishTotal = FindFishInTank(fish.fishSO.fishName);

                    fishAmountText[s].text = 
                        $"{fish.fishSO.fishName}: {fishTotal}";
                }
                else
                {
                    return;
                }
            }
        }
    }

    public int FindFishInTank(string fishToFind)
    {
        int fishCount = 0;
        List<Fish> fishList = new List<Fish>();
        foreach (GameObject gameObject in GameManager.instance.fishActive)
        {
            if (gameObject.TryGetComponent<Fish>(out Fish fish))
            {
                fishList.Add(fish);
            }
        }
        foreach (Fish f in fishList)
        {
            if (f.fishSO.fishName == fishToFind)
            {
                fishCount++;
            }
        }

        return fishCount;
    }
}
