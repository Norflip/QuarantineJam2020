using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

[RequireComponent(typeof(ObjectData))]
public class PickableObject : MonoBehaviour
{
    const float SplitForce = 40.0f;

    public enum ObjectType
    {
        Object,
        Tool
    }

    public ObjectType type = ObjectType.Object;

    public bool DropOnUse => type == ObjectType.Object;

    [Space(10.0f)]
    public bool changeRotationOnPickup = false;

    public bool Broken => m_broken;
    bool m_broken = false;


    public Rigidbody Rigidbody => rb;
    Rigidbody rb;

    public ObjectData ObjectData => od;
    ObjectData od;

    protected virtual void Awake()
    {
        od = GetComponent<ObjectData>();
        rb = GetComponent<Rigidbody>();

        od.RecalculateMeshVolume();
        rb.mass = od.Mass;
    }

    public void MarkAsBroken() => m_broken = true;

    public virtual void UseObject(Transform user, float force)
    {
        rb.AddForce(user.forward * force, ForceMode.Impulse);
    }

    public bool TrySlice(Vector3 position, Vector3 normal, bool selfDestroy, out PickableObject[] objects)
    {
        bool split = false;
        objects = null;
        normal.Normalize();

        if (od.materialData.breakable)
        {
            Debug.Log("N1: " + normal.ToString() + " N2: " + transform.InverseTransformDirection(normal).ToString());

            Vector3 localPosition = transform.InverseTransformPoint(position);
            Vector3 localNormal = transform.InverseTransformDirection(normal);
            EzySlice.Plane plane = new EzySlice.Plane(localPosition, localNormal);

            GameObject[] objs = gameObject.SliceInstantiate(plane, new TextureRegion(0,0,1,1), od.materialData.crossSectionMaterial);

            if(objs != null)
            {
                Debug.Log(objs.Length);
                objects = new PickableObject[objs.Length];

                for (int i = 0; i < objs.Length; i++)
                    objects[i] = CreateSplitPiece(objs[i], position, normal);

                if (selfDestroy)
                    Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("failed to split @pos: " + position.ToString() + " normal: " + normal.ToString());
            }
        }

        return split;
    }

    PickableObject CreateSplitPiece (GameObject tmp, Vector3 planePosition, Vector3 planeNormal)
    {
        PickableObject upper = Instantiate(this);
        upper.gameObject.SetActive(false);

        MeshRenderer mr = upper.GetComponent<MeshRenderer>();
        mr.materials = tmp.GetComponent<MeshRenderer>().materials;

        upper.transform.position = tmp.transform.position;
        upper.transform.rotation = tmp.transform.rotation;
        upper.transform.localScale = tmp.transform.localScale;
        upper.transform.SetParent(transform.parent);

        Mesh m = tmp.GetComponent<MeshFilter>().mesh;

        upper.GetComponent<MeshFilter>().mesh = m;
        upper.GetComponent<MeshCollider>().sharedMesh = m;
        upper.GetComponent<ObjectData>().RecalculateMeshVolume();
        upper.MarkAsBroken();

        Destroy(tmp);
        upper.gameObject.SetActive(true);

        float side = Mathf.Sign(Vector3.Dot(planeNormal, (upper.transform.position - planePosition).normalized));
        upper.Rigidbody.AddForce(planeNormal * side * SplitForce);
        return upper;
    }

    public void EnablePhysics(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = state;

        rb.isKinematic = !state;
    }
}
