using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class HookPing : MonoBehaviour
{
    [SerializeField] float duration;
    SpriteRenderer sr;
    private float startAlpha;
    private float targetAlpha = 1f;
    private float timeElapsed = 0;

    [Header("Ping Animation Stuff")]
    [SerializeField] float bobSpeed;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startAlpha = sr.color.a;
    }

    void Update()
    {
        FadePing();
        MovePing();
    }

    void FadePing()
    {
        if (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            Color color = sr.color;
            color.a = newAlpha;
            sr.color = color;
        }
    }

    void MovePing()
    {
        float bobbingValue = Mathf.PingPong(Time.time * bobSpeed, 1f);
        float newY = Mathf.Lerp(minY, maxY, bobbingValue);
        transform.localPosition = new Vector2(transform.localPosition.x, newY);
    }
}
