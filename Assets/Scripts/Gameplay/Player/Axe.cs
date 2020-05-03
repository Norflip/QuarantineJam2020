using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public const float SEPERATION_FORCE = 5.0f;

    public LayerMask destroyableMask;
    public float swingTime = 0.2f;
    public float swingCooldown = 0.7f;
    public float maxSwingDistance = 4.5f;

    public float arc = 45.0f;
    public int raycastCount = 10;

    [HideInInspector]
    public Transform owner;

    float lastSwing = -1;
    Animator animator;
    Transform user;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (lastSwing == -1 || lastSwing + swingCooldown < Time.time)
            {
                StartCoroutine(Swing(owner));
                lastSwing = Time.time;
            }
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
        Debug.Log("SLICING");

        Ray rr = new Ray(user.position, user.forward);
        RaycastHit[] hh = Physics.SphereCastAll(rr, 0.1f, maxSwingDistance, destroyableMask.value);
        Slicable slicee;

        List<Slicable> slicees = new List<Slicable>();
        HashSet<Slicable> visited = new HashSet<Slicable>();

        for (int i = 0; i < hh.Length; i++)
        {
            if (hh[i].transform != transform)
            {
                slicee = hh[i].transform.GetComponent<Slicable>();

                if (slicee != null && slicee.materialData.breakable && !visited.Contains(slicee))
                {
                    slicees.Add(slicee);
                    visited.Contains(slicee);
                }
            }
        }

        Debug.Log(slicees.Count);

        for (int i = 0; i < slicees.Count; i++)
        {
            Debug.Log(slicees[i].name);
            slicees[i].TrySlice(user.position, user.right, true, out _);
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
