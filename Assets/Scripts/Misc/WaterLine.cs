using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLine : MonoBehaviour
{
    BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Fish>(out Fish fish))
        {
            if (fish.fishState.GetCurrentState() == FishState.State.Spawning)
            {
                SFXManager.instance.PlaySFX(SoundType.Splash);
            }
        }
    }
}
