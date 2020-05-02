using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CollectionManager : MonoSingleton<CollectionManager>
{
    public const float BROKEN_MODIFIER = 0.2f;
    public string collectionLayer = "Collection";

    List<float> points = new List<float>();
    public Transform container;

    public void AddObject (Slicable obj)
    {
        GameObject clone = new GameObject("Clone");
        clone.AddComponent<MeshFilter>().sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
        clone.AddComponent<MeshRenderer>().sharedMaterials = obj.GetComponent<MeshRenderer>().sharedMaterials;
        clone.AddComponent<MeshCollider>().sharedMesh = obj.GetComponent<MeshCollider>().sharedMesh;
        clone.layer = LayerMask.NameToLayer(collectionLayer);
        clone.transform.SetParent(container);

        clone.transform.position = obj.transform.position;
        clone.transform.localScale = obj.transform.localScale;
        clone.transform.rotation = obj.transform.rotation;



        float pp = CalculatePoints(obj);
        points.Add(pp);
        Destroy(obj.gameObject);

        Debug.Log("POINTS ADDED: " + pp);
    }

    int CalculatePoints (Slicable obj)
    {
        float value = (obj.materialData.weight * obj.MeshVolume) * Mathf.Max(1.0f, (int)obj.materialData.rarity / 100.0f) * 100.0f;
        if (obj.Broken) value *= BROKEN_MODIFIER;
        return Mathf.RoundToInt(value);
    }

    public float PointsSum ()
    {
        float sum = 0.0f;
        for (int i = 0; i < points.Count; i++)
            sum += points[i];
        
        return sum;
    }
}
