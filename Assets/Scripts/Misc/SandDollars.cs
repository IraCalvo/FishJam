using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SandDollars : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void Start()
    {
        if (text != null)
        {
            text.text = "Sand Dollars: " + PlayerPrefs.GetInt("Sand Dollar", 0);
        }
    }
}
