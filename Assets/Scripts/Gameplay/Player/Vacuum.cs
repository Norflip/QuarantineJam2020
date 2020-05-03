using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.SearchableEnum;
using System;

public class Vacuum : MonoBehaviour
{
    const float PickSphereRadius = 0.4f;

    public LayerMask pickupLayer;
    public string pickupLayerName = "Pickable";
    public string cameraLayerName = "PickupCamera";

    [Space(10.0f)]
    public float pickMaxDistance = 1.5f;
    public float throwForce = 20.0f;
    public float maxCapacity = 60.0f;

    public float windupTime = 0.7f;
    public float outputCooldown = 0.6f;

    public float suckForce = 0.2f;

    [Space(10.0f)]
    public string propKey = "planeHeight";
    public Material liquidMat;

    [Space(10.0f)]
    public Transform hands;

    [Header("Animation")]
    public float lerpTime = 0.3f;
    public AnimationCurve lerpCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public List<Slicable> holding;
    public float currentCapacity;


    [HideInInspector]
    public Transform owner;

    Slicable holdee;
    Transform previousParent;
    Camera cam;
    ThrowArc arc;

    float nextOutputTime;

    float liquidLevelVelocity;
    float targetLiquidLevel;
    float currentLiquidLevel;

    private void Awake()
    {
        cam = Camera.main;
        arc = GetComponent<ThrowArc>();
        currentCapacity = 0.0f;
        nextOutputTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            arc.Show(false);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            // suck in
            //show suck in effects

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.SphereCast(ray, PickSphereRadius, out hit, pickMaxDistance, pickupLayer.value))
            {
                GameObject go = hit.transform.gameObject;
                holdee = go.GetComponent<Slicable>();

                if (CanSuckObject(holdee))
                {
                    holding.Add(holdee);
                    currentCapacity += holdee.Mass;

                    StartCoroutine(MoveToHands(holdee));
                }
                else
                {
                    Debug.Log("Cant lift object");
                    OnFailedSuck();
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            //arc.SetParams(cam.transform.position + cam.transform.right * 0.2f, cam.transform.forward * throwForce, holdee.MeshVolume * holdee.materialData.weight, Physics.gravity);

            if (holding.Count > 0 && nextOutputTime < Time.time)
            {
                nextOutputTime = Time.time + holding[0].MeshVolume * outputCooldown;

                if (holding.Count > 0)
                {
                    UseObject(holding[0]);

                    currentCapacity -= holding[0].Mass;
                    currentCapacity = Math.Max(0, currentCapacity);
                    holding.RemoveAt(0);
                }
                else
                {
                    OnEmptyOut();
                }
            }
        }
        else
        {
            arc.Show(false);
        }

        UpdateBulb();
    }

    void UpdateBulb ()
    {
        targetLiquidLevel = currentCapacity / maxCapacity;
        currentLiquidLevel = Mathf.SmoothDamp(currentLiquidLevel, targetLiquidLevel, ref liquidLevelVelocity, 0.1f);
        liquidMat.SetFloat(propKey, Mathf.Lerp(0.5f, 1.0f, currentLiquidLevel));
    }

    void RaycastObjects()
    {

    }

    void OnFailedSuck()
    {

    }

    void OnEmptyOut()
    {

    }

    bool CanSuckObject(Slicable po)
    {
        return po != null && po.Mass + currentCapacity < maxCapacity;
    }

    void UseObject(Slicable hh)
    {
        hh.gameObject.SetActive(true);

        AttachObjectToHand(false, hh);
        hh.EnablePhysics(true);

        hh.OnLeftClick(cam.transform, throwForce);
        arc.Show(false);
    }

    void AttachObjectToHand(bool state, Slicable go)
    {
        if (state)
        {
            previousParent = go.transform.parent;
            go.transform.SetParent(hands);
            go.transform.localPosition = Vector3.zero;
            go.gameObject.SetActive(false);
        }
        else
        {
            go.transform.SetParent(previousParent, true);
            go.transform.localScale = Vector3.one;
            go.gameObject.SetActive(true);
        }
    }

    IEnumerator MoveToHands(Slicable go)
    {
        if (go == null)
            yield break;

        Vector3 startWorld = go.transform.position;
        Vector3 startScale = go.transform.localScale;
        Quaternion startRotation = go.transform.rotation;
        go.EnablePhysics(false);

        float t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime / lerpTime;
            float tt = lerpCurve.Evaluate(t);

            go.transform.position = Vector3.Lerp(startWorld, hands.position, tt);
            go.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, tt);

            if (go.changeRotationOnPickup)
                go.transform.rotation = Quaternion.Lerp(startRotation, hands.rotation, tt);

            yield return null;
        }

        AttachObjectToHand(true, go);
    }

}
