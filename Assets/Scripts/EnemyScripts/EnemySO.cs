using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public float MaxHP;
    public float speed;
    public int damage;
    public float attackRange;
    public PoolObjectType enemyPoolObjectType;
}
