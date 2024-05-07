using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    TextMeshProUGUI text;
    public float fadeDuration;
    private Renderer objectRenderer;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        objectRenderer = GetComponent<Renderer>();
    }

    private void OnDisable()
    {
        ResetPopup();
    }

    public void Setup(int damageToShow)
    {
        text.text = damageToShow.ToString();
        StartCoroutine(FadeAwayCoroutine());
    }

    IEnumerator FadeAwayCoroutine()
    {
        float timer = fadeDuration;
        while (timer > 0f)
        {
            float alphaAmount = timer / fadeDuration;
            alphaAmount = Mathf.Clamp01(alphaAmount);

            Color objectColor = objectRenderer.material.color;
            objectColor.a = alphaAmount;
            objectRenderer.material.color = objectColor;

            timer -= Time.deltaTime;

            yield return null;
        }

        PoolManager.instance.DeactivateObjectInPool(gameObject);
    }

    private void ResetPopup()
    {
        Color objColor = objectRenderer.material.color;
        objColor.a = 1f;
        objectRenderer.material.color = objColor;
    }
}
