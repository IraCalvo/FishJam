using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectArrow : MonoBehaviour
{
    [SerializeField] float bobSpeed;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    private void Update()
    {
        float bobbingValue = Mathf.PingPong(Time.time * bobSpeed, 1f);
        float newY = Mathf.Lerp(minY, maxY, bobbingValue);
        transform.localPosition = new Vector2(transform.localPosition.x, newY);
    }
}
