using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMaterialManager : MonoBehaviour
{
    [SerializeField] public Material defaultMaterial;
    [SerializeField] public Material selectedMaterial;
    [SerializeField] public Material hungerMaterial;
    [SerializeField] public Material deadMaterial;

    private Fish fish;
    private FishState fishState;
    private SpriteRenderer sr;

    private void Awake()
    {
        fish = GetComponent<Fish>();
        fishState = fish.fishState;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (fishState.GetCurrentState())
        {
            case FishState.State.Normal:
                sr.material = defaultMaterial;
                break;
            case FishState.State.Hungry:
                sr.material = hungerMaterial;
                break;
            case FishState.State.Dead: 
                sr.material = deadMaterial; 
                break;
        }
        if (BoxSelection.Instance.ContainsFish(fish))
        {
            sr.material = selectedMaterial;
        }
    }
}
