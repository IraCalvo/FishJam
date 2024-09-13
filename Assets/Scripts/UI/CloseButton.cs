using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    [SerializeField] List<GameObject> thingsToClose;

    public void ButtonPressed()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        for (int i = 0; i < thingsToClose.Count; i++)
        {
            thingsToClose[i].SetActive(false);
        }
    }
}
