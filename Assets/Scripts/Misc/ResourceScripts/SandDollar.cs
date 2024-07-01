using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandDollar : Resource
{
    public override void AbstractAwake()
    {
        throw new System.NotImplementedException();
    }

    public override void AbstractFixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    public override void ResourceClicked()
    {
        Debug.Log("Called");
        int currentValue = PlayerPrefs.GetInt("Sand Dollar");
        int newValue = currentValue + resourceSO.resourceValue;
        PlayerPrefs.SetInt("Sand Dollar" , newValue);
        PoolManager.instance.DeactivateObjectInPool(gameObject);
        SFXManager.instance.PlaySFX(SoundType.SandDollar);
    }
}
