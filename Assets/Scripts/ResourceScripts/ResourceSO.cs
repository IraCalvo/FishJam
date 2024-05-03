using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResourceSO")]
public class ResourceSO : ScriptableObject
{
    public string resourceName;
    public int resourceValue;
    public PoolObjectType resourcePool;
    public float disappearTime;
}
