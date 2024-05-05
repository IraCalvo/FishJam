using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSelection : MonoBehaviour
{
    [SerializeField] Material startingMaterial;
    [SerializeField] Material selectionMaterial;
    [SerializeField] SpriteRenderer sr;
    public bool isSelected = false;

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Selector")
        {
            isSelected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Selector")
        {
            isSelected = false;
        }
    }
}
