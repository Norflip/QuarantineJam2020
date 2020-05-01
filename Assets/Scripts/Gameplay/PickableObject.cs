using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectData))]
public class PickableObject : MonoBehaviour
{
    public bool broken = false;

    ObjectData od;

    public Rigidbody Rigidbody => rd;
    Rigidbody rd;

    private void Awake()
    {
        od = GetComponent<ObjectData>();
        rd = GetComponent<Rigidbody>();

        od.RecalculateMeshVolume();
        rd.mass = od.Mass;
    }

    public void EnablePhysics (bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = state;

        rd.isKinematic = !state;
    }
}
