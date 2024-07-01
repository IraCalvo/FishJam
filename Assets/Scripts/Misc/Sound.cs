using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public SoundType soundType;
    public AudioClip[] clip;
    [Range(0.01f, 1f)]
    public float volume;
}
