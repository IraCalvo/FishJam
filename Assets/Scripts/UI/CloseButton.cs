using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    [SerializeField] GameObject thingToClose;

    public void ButtonPressed()
    {
        SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
        thingToClose.SetActive(false);
    }
}
