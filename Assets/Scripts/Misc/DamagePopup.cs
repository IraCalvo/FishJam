using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    TextMeshPro text;
    public float fadeDuration;
    private MeshRenderer objectRenderer;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        objectRenderer = GetComponent<MeshRenderer>();
        objectRenderer.sortingOrder = 10;
    }

    private void OnDisable()
    {
        ResetPopup();
    }

    public void Setup(int damageToShow)
    {
        text.text = damageToShow.ToString();
    }

    public void AnimEventStartFade()
    {
        StartCoroutine(FadeAwayCoroutine());
    }

    IEnumerator FadeAwayCoroutine()
    {
        float startAlpha = text.color.a;
        float currentTime = 0.0f;
        while (currentTime < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, 0f, currentTime / fadeDuration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, newAlpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        PoolManager.instance.DeactivateObjectInPool(gameObject);
    }

    private void ResetPopup()
    {
        Color textColor = text.color;
        textColor.a = 1f;
        text.color = textColor;
    }
}
