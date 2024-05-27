using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFX : MonoBehaviour
{
    ParticleSystem bloodFX;

    public void Awake()
    {
        bloodFX = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        bloodFX.Play();
    }

    private void Update()
    {
        if (bloodFX.isPlaying == false)
        { 
            PoolManager.instance.DeactivateObjectInPool(gameObject);
        }
    }
}
