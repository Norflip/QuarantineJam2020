using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectData))]
public class PickableObject : MonoBehaviour
{
    public bool broken = false;

    ObjectData od;
    Rigidbody rd;

    private void Awake()
    {
        od = GetComponent<ObjectData>();
        rd = GetComponent<Rigidbody>();

        od.RecalculateMeshVolume();
        rd.mass = od.Mass;
    }

    public void EnablePhysics ()
    {
       // this.rd
    }
}
