using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float fadeDuration;
    public PoolObjectType fadeObjectPoolType;

    private float fadeTimer;
    private Renderer objectRender;

    private void Awake()
    {
        objectRender = GetComponent<Renderer>();
        fadeTimer = fadeDuration;
    }

    public void StartFade()
    {
        StartCoroutine(FadeAwayCoroutine());
    }

    public IEnumerator FadeAwayCoroutine()
    {
        float timer = fadeDuration;
        while (timer > 0f)
        {
            float alphaAmount = timer / fadeDuration;

            alphaAmount = Mathf.Clamp01(alphaAmount);

            Color objectColor = objectRender.material.color;
            objectColor.a = alphaAmount;
            objectRender.material.color = objectColor;

            timer -= Time.deltaTime;

            yield return null;
        }

        PoolManager.instance.DeactivateObjectInPool(this.gameObject, fadeObjectPoolType);
    }
}
