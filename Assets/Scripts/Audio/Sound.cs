using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Effect,
    Ambience,
    Music
}

[System.Serializable]
public class Sound
{
    public string key;
    public AudioClip clip;

    public SoundType type = SoundType.Effect;
    
    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Range(-3f, 3f)]
    public float pitch = 1.0f;

    [Range(0f, 1f)]
    public float spatialBlend = 1.0f;

    public bool loop = false;
    public bool oneShot = false;
}

[System.Serializable]
public class SoundGroup
{
    public string key = "";
    public string[] variationKeys = new string[0];
}
