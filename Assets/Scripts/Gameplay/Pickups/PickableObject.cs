using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PickableObject : MonoBehaviour
{
    public MaterialData materialData;
    public bool changeRotationOnPickup = false;

    public abstract bool DropOnUse { get; }
    
    public Rigidbody Rigidbody
    {
        get
        {
            if (m_rb == null) m_rb = GetComponent<Rigidbody>();
            return m_rb;
        }
    }

    Rigidbody m_rb;

    public float MeshVolume => meshVolume;
    float meshVolume;

    [HideInInspector]
    public float OriginalMeshVolume = -1;

    protected virtual void Awake()
    {
        RecalculateVolume();
    }

    public void RecalculateVolume ()
    {
        meshVolume = MeshData.CalculateMeshVolume(GetComponent<MeshFilter>().sharedMesh, transform.localScale);
        OriginalMeshVolume = meshVolume;
        Rigidbody.mass = meshVolume * materialData.density;
    }

    public abstract void OnLeftClick(Transform user, float force);

    public void EnablePhysics(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = state;

        m_rb.isKinematic = !state;
    }
}
