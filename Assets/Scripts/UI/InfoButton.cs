using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    public Image infoImage;
    public void ButtonPressed()
    {
        infoImage.gameObject.SetActive(true);
    }
}
