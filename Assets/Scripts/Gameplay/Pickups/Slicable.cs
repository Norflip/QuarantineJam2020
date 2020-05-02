using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityMeshSimplifier;
using System;

[RequireComponent(typeof(MeshCollider))]
public class Slicable : PickableObject
{
    public const float MINIMUM_STICK_VELOCITY = 0.1f;
    public const float REQUIRED_FREEZE_TIME = 0.2f;
    public const float LIFETIME = 24.0f;

    public float freezeTimer = 0.0f;
    public float lastFreezeTime = -1;

    public enum SlicableState
    {
        Freezed,

    }

    const float SplitForce = 10.0f;
    public string collectionLayer = "Collection";

    public bool Broken => m_broken;
    public override bool DropOnUse => true;

    bool active = false;
    bool m_broken = false;
    SlicableState state;

    protected override void Awake()
    {
        base.Awake();
        state = SlicableState.Freezed;
    }

    public void OnSplit()
    {
        m_broken = true;
        RecalculateVolume();
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime ()
    {
        yield return new WaitForSeconds(LIFETIME);
        Destroy(gameObject);
    }

    public bool TrySlice(Vector3 position, Vector3 normal, bool selfDestroy, out Slicable[] objects)
    {
        bool split = false;
        objects = null;

        if (materialData.breakable)
        {
            normal.Normalize();
            Vector3 localPosition = transform.InverseTransformPoint(position);
            Vector3 localNormal = transform.InverseTransformDirection(normal);
            EzySlice.Plane plane = new EzySlice.Plane(localPosition, localNormal);
        
            GameObject[] objs = gameObject.SliceInstantiate(plane, new TextureRegion(0, 0, 1, 1), materialData.crossSectionMaterial);

            if (objs != null)
            {
                objects = new Slicable[objs.Length];
                for (int i = 0; i < objs.Length; i++)
                    objects[i] = CreateSplitPiece(objs[i], position, normal);

                Destroy(gameObject);
                split = true;
            }
            else
            {
                Debug.LogWarning("failed to split @pos: " + position.ToString() + " normal: " + normal.ToString());
            }
        }

        return split;
    }

    Slicable CreateSplitPiece(GameObject tmp, Vector3 planePosition, Vector3 planeNormal)
    {
        Slicable upper = Instantiate(this);

        MeshRenderer mr = upper.GetComponent<MeshRenderer>();
        mr.materials = tmp.GetComponent<MeshRenderer>().materials;

        //upper.transform.position = tmp.transform.position;
        //upper.transform.rotation = tmp.transform.rotation;
        //upper.transform.localScale = tmp.transform.localScale;
        upper.transform.SetParent(transform.parent);

        Mesh m = tmp.GetComponent<MeshFilter>().mesh;
        upper.GetComponent<MeshFilter>().mesh = m;
        Destroy(tmp);

        MeshCollider mc = upper.GetComponent<MeshCollider>();
        mc.convex = true;
        //mc.sharedMesh = null;
        mc.sharedMesh = m;

        upper.OnSplit();

        float side = Mathf.Sign(Vector3.Dot(planeNormal, (upper.transform.position - planePosition).normalized));
        upper.Rigidbody.AddForce(planeNormal * side * SplitForce);
        return upper;
    }

    public override void OnLeftClick(Transform user, float force)
    {
        this.Rigidbody.AddForce(user.forward * force, ForceMode.Impulse);
        this.active = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(collectionLayer))
        {
            float sqrv = Rigidbody.velocity.sqrMagnitude;

            if (sqrv < MINIMUM_STICK_VELOCITY * MINIMUM_STICK_VELOCITY)
            {
                freezeTimer += Time.fixedDeltaTime;
            }
            else
            {
                freezeTimer = 0.0f;
            }

            if(freezeTimer >= REQUIRED_FREEZE_TIME)
            {
                Debug.Log(sqrv + " < " + (MINIMUM_STICK_VELOCITY * MINIMUM_STICK_VELOCITY));
                CollectionManager.Instance.AddObject(this);
                return;
            }
        }
    }

    public Slicable CreateHull(Mesh mm)
    {
        if (mm = null)
            return null;

        Slicable newObject = Instantiate(this);

        if (newObject != null)
        {
            newObject.GetComponent<MeshFilter>().mesh = mm;

            MeshCollider mc = newObject.GetComponent<MeshCollider>();
            mc.convex = true;
            mc.sharedMesh = null;
            mc.sharedMesh = mm;

            newObject.transform.localPosition = transform.localPosition;
            newObject.transform.localRotation = transform.localRotation;
            newObject.transform.localScale = transform.localScale;

            Material[] shared = GetComponent<MeshRenderer>().sharedMaterials;
            Mesh og_mesh = GetComponent<MeshFilter>().mesh;

            //// nothing changed in the hierarchy, the cross section must have been batched
            //// with the submeshes, return as is, no need for any changes
            //if (og_mesh.subMeshCount == mm.subMeshCount)
            //{
            //    // the the material information
            //    newObject.GetComponent<Renderer>().sharedMaterials = shared;
            //    return newObject;
            //}

            // otherwise the cross section was added to the back of the submesh array because
            // it uses a different material. We need to take this into account
            Material[] newShared = new Material[shared.Length + 1];

            // copy our material arrays across using native copy (should be faster than loop)
            System.Array.Copy(shared, newShared, shared.Length);
            newShared[shared.Length] = materialData.crossSectionMaterial;

            // the the material information
            newObject.GetComponent<Renderer>().sharedMaterials = newShared;
        }

        return newObject;
    }
}
