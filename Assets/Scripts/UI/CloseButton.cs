using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    [SerializeField] GameObject thingToClose;

    public void ButtonPressed()
    {
        Debug.Log("Detected");
        thingToClose.SetActive(false);
    }
}
