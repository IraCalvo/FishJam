using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance { get; private set; }
    [SerializeField] private Item currentItem;

    private void Awake()
    {
        Instance = this;
    }

    public Item GetCurrentItem() { return currentItem; }
}
