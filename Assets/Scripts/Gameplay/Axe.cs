using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : PickableObject
{
    public LayerMask destroyableMask;
    public float swingTime = 0.2f;
    public float swingCooldown = 0.7f;
    public float maxSwingDistance = 1.5f;

    public float arc = 45.0f;
    public int raycastCount = 10;

    float lastSwing = -1;
    Animator animator;
    Transform user;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void UseObject(Transform user, float force)
    {
        if (lastSwing == -1 || lastSwing + swingCooldown < Time.time)
        {
            this.user = user;
            StartCoroutine(Swing(user));
            lastSwing = Time.time;
        }
    }

    IEnumerator Swing(Transform user)
    {
        // start animation
        float t = 0.0f;
        bool passedHalf = false;
        animator.Play("Swing");

        while (t <= 1.0f)
        {
            t += Time.deltaTime / swingTime;

            if (!passedHalf && t >= 0.5f)
            {
                SplitObjects(user);
                passedHalf = true;
            }

            yield return null;
        }
    }

    void SplitObjects(Transform user)
    {
        Ray rr = new Ray(user.position, user.forward);
        RaycastHit[] hh = Physics.SphereCastAll(rr, 0.1f, 5.0f, destroyableMask.value);

        //Debug.Log("HH: " + hh.Length);

        for (int i = 0; i < hh.Length; i++)
        {
            if(hh[i].transform != transform)
            {
                PickableObject po = hh[i].transform.GetComponent<PickableObject>();
                if(po != null && po.ObjectData.materialData.breakable)
                {
                    PickableObject[] newObjects;
                    po.TrySlice(user.position, user.right, true, out newObjects);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        float anglePerRay = (float)arc / (float)raycastCount;

        for (int i = 0; i < raycastCount; i++)
        {
            float angle = ((i * anglePerRay) - (arc / 2.0f)) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(0.0f, Mathf.Sin(angle), Mathf.Cos(angle)).normalized;

            if (user == null)
            {
                dir = transform.rotation * dir;

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, transform.position + dir * maxSwingDistance);
            }
            else
            {
                dir = user.rotation * dir;
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(user.position, user.position + dir * maxSwingDistance);
            }
        }

    }
}
