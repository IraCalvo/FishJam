using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchStoreButton : MonoBehaviour
{
    [SerializeField] GameObject fishStore;
    [SerializeField] GameObject itemStore;

    public void SwitchStore()
    {
        if (fishStore.activeInHierarchy)
        {
            fishStore.SetActive(false);
            itemStore.SetActive(true);
        }
        else
        {
            fishStore.SetActive(true);
            itemStore.SetActive(false);
        }
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
    }
}
