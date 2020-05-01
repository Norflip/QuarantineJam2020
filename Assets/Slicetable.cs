using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Slicetable : MonoBehaviour
{
    public Vector3 planeNormal = new Vector3(1, 0, 0);
    public GameObject slicee;
    public Material crossSection;

    private void Start()
    {
        if(slicee == null)
        {
            Debug.LogError("no slicee");
            return;
        }

        GameObject[] gos = slicee.SliceInstantiate(slicee.transform.position, planeNormal.normalized, crossSection);

        Debug.Log("Created " + (gos != null ? gos.Length : 0) + "");
        Destroy(slicee.gameObject);
    }
}
