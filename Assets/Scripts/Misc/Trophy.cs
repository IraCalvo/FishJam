using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trophy : MonoBehaviour
{
    [SerializeField] bool isMainLevel;
    [SerializeField] int levelIndex;
    public int price = 5000;
    [SerializeField] GameObject winScreen;

    public void BuyTrophy()
    {
        if (BankManager.Instance.CanAfford(price))
        {
            BankManager.Instance.RemoveMoney(price);
            winScreen.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("Congrats you won");
            UnlockNewLevel();
        }
    }

    void UnlockNewLevel()
    {
        if (isMainLevel && levelIndex > PlayerPrefs.GetInt("HighestLevel", 0))
        {
            PlayerPrefs.SetInt("HighestLevel", levelIndex + 1);
        }
    }
}
