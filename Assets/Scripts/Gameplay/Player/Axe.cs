using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using CielaSpike;

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
        animator.Play("Swing");
        SplitObjects(user);
        yield return new WaitForSeconds(swingTime);
    }
 

    void SplitObjects(Transform user)
    {
        Ray rr = new Ray(user.position, user.forward);

        RaycastHit[] hh = Physics.SphereCastAll(rr, 0.1f, maxSwingDistance, destroyableMask.value);
        Slicable slicee;

        HashSet<Slicable> visited = new HashSet<Slicable>();

        for (int i = 0; i < hh.Length; i++)
        {
            if (hh[i].transform != transform)
            {
                slicee = hh[i].transform.GetComponent<Slicable>();

                if (slicee != null && slicee.materialData.breakable && !visited.Contains(slicee))
                {
                    //slicees.Add(slicee);
                    slicee.TrySlice(user.position, user.right, true, out _);

                    visited.Contains(slicee);
                }

                break;
            }
        }
    }
}
