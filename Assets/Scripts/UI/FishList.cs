using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class FishList : MonoBehaviour
{
    public static FishList instance;
    public bool fishListIsActive;
    public List<TextMeshProUGUI> fishAmountText;
    public List<Image> fishRenders;

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

                    fishAmountText[s].text = $":{fishTotal}";
                    //$"{fish.fishSO.fishName}: {fishTotal}";

                    fishRenders[s].sprite = shopItem.image.sprite;
                    if (fish.fishSpecies == FishSpecies.Angler)
                    {
                        RectTransform rt = fishRenders[s].GetComponent<RectTransform>();
                        rt.localScale = new Vector3(2.75f, 1.75f, 1);
                        rt.localPosition = new Vector3(-120, rt.localPosition.y, rt.localPosition.z);
                    }
                    if (fish.fishSpecies == FishSpecies.Crab 
                        || fish.fishSpecies == FishSpecies.Shark 
                        || fish.fishSpecies == FishSpecies.Pirahna)
                    {
                        RectTransform rt = fishRenders[s].GetComponent<RectTransform>();
                        rt.localScale = new Vector3(1.5f, 1.5f, 1);
                    }
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

        for (int i = 0; i < GameManager.instance.activeFish.Count; i++)
        {
            if (GameManager.instance.activeFish[i].fishSO.fishName == fishToFind)
            {
                fishCount++;
            }
        }

        return fishCount;
    }
}

//PoolInfo fishpoolInfo = PoolManager.instance.GetPoolByFishName(fishToFind);
//if (fishpoolInfo != null)
//{
//    foreach (GameObject fish in fishpoolInfo.pool)
//    {
//        if (fish.activeInHierarchy)
//        {
//            fishCount++;
//        }
//    }
//}
