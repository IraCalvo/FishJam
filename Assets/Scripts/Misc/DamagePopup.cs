using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    TextMeshPro text;
    public float fadeDuration;
    private MeshRenderer objectRenderer;
    Animator anim;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        objectRenderer = GetComponent<MeshRenderer>();
        objectRenderer.sortingOrder = 100;
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.SetTrigger("StartAnim");
    }

    private void OnDisable()
    {
        ResetPopup();
    }

    public void Setup(int damageToShow)
    {
        text.text = damageToShow.ToString();
    }

    public void AnimEventDisableObj()
    {
        PoolManager.instance.DeactivateObjectInPool(gameObject);
    }

    private void ResetPopup()
    {
        Color textColor = text.color;
        textColor.a = 1f;
        text.color = textColor;
    }
}
