﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CollectionManager : MonoSingleton<CollectionManager>
{
    public const float BROKEN_MODIFIER = 0.2f;
    public string collectionLayer = "Collection";
    public Color freezeColor = new Color(0, 0, 1);
    [Range(0f, 1f)] public float freezeColorFactor = 0.5f;


    public int PointSum => sum;
    int sum;

    public Transform container;

    private void Awake()
    {
        sum = 0;
    }

    public void AddObject(Slicable obj)
    {
        GameObject clone = new GameObject("Clone");
        clone.AddComponent<MeshFilter>().sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Material[] oldMats = obj.GetComponent<MeshRenderer>().materials;
        Material[] mats = new Material[oldMats.Length];

        for (int i = 0; i < oldMats.Length; i++)
        {
            Material m = oldMats[i];

            if (oldMats[i].HasProperty("Color_75A9C323"))
            {
                Color c = m.GetColor("Color_75A9C323");
                m.color = Color.Lerp(c, freezeColor, freezeColorFactor);
            }

            mats[i] = m;
        }

        clone.AddComponent<MeshRenderer>().sharedMaterials = mats;
        clone.AddComponent<MeshCollider>().sharedMesh = obj.GetComponent<MeshCollider>().sharedMesh;
        clone.transform.SetParent(container);
        clone.transform.position = obj.transform.position;
        clone.transform.localScale = obj.transform.localScale;
        clone.transform.rotation = obj.transform.rotation;

        int pp = CalculatePoints(obj);
        sum += pp;

        Destroy(obj.gameObject);
    }

    int CalculatePoints(Slicable obj)
    {
        float value = (obj.materialData.weight * obj.MeshVolume) * Mathf.Max(1.0f, (int)obj.materialData.rarity / 100.0f) * 10.0f;
        if (obj.Broken) value *= BROKEN_MODIFIER;
        return Mathf.RoundToInt(value);
    }
}
