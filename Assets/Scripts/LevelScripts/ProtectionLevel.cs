using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionLevel : MonoBehaviour
{
    public static ProtectionLevel instance;
    [SerializeField] List<StarterFish> fishToProtect;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void RemoveFishFromList(StarterFish f)
    { 
        fishToProtect.Remove(f);
        CheckGameLose();
    }

    void CheckGameLose()
    {
        if (fishToProtect.Count == 0)
        { 
            GameManager.instance.PlayerLose();
        }
    }
}
