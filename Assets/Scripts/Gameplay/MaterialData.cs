using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Material Data")]
public class MaterialData : ScriptableObject
{
    public enum ObjectRarityValue
    {
        Common = 100, 
        Rare = 165,
        Epic = 220
    }

    public ObjectRarityValue rarity;
    public float weight = 1.0f;

    public bool breakable = true;
    public Material crossSectionMaterial;

    public string soundKey = "";
    public bool playAsGroup = false;
}
