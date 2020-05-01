using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Material Data")]
public class MaterialData : ScriptableObject
{
    public enum ObjectRarityValue
    {
        Common,
        Rare,
        Epic
    }

    public ObjectRarityValue rarity;
    public float density = 1.0f;
}
