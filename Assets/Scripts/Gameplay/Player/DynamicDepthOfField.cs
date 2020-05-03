#define DYNAMIC_DEPTH_OF_FIELD

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;


public class DynamicDepthOfField : MonoBehaviour
{


    public Volume iv;
    public float focusSpeed = 8.0f;
    public float maxFocusDistance;
    public LayerMask interactionMask;

    public bool HasFocus => m_hasFocus;
    public float FocusDistance => m_hitDistance;
    public Vector3 FocusPoint => raycastHit.point;

    Ray viewRaycast;
    RaycastHit raycastHit;

    bool m_hasFocus;
    float m_hitDistance;

    DepthOfField depthOfField;

    private void Start()
    {
        if (iv != null)
            iv.profile.TryGet(out depthOfField);

        //postProcessVolume.profile.TryGetSettings(out depthOfField);
    }

    public void UpdateDepthOfField()
    {
        if (iv != null)
            return;

        viewRaycast = new Ray(transform.position, transform.forward * maxFocusDistance);
        m_hasFocus = false;

        if (Physics.Raycast(viewRaycast, out raycastHit, maxFocusDistance, interactionMask.value))
        {
            m_hasFocus = true;
            m_hitDistance = Vector3.Distance(transform.position, raycastHit.point);
        }
        else
        {
            if (m_hitDistance < maxFocusDistance)
            {
                m_hitDistance++;
            }
        }

        SetFocus();
    }

    void SetFocus()
    {
        //depthOfField.focusDistance.value = Mathf.Lerp(depthOfField.focusDistance.value, m_hitDistance, focusSpeed * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        if (HasFocus)
        {
            Vector3 p = FocusPoint;
            Gizmos.DrawWireSphere(p, 0.1f);
            Debug.DrawRay(transform.position, transform.forward * Vector3.Distance(transform.position, p));
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * maxFocusDistance);
        }

        //Gizmos.DrawRay(transform.position, transform.position + transform.forward * m_hitDistance);
        //Gizmos.DrawSphere(raycastHit.point, 0.1f);
    }
}
