using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.SearchableEnum;

public class PickupController : MonoBehaviour
{
    const float PickSphereRadius = 0.3f;

    public LayerMask pickupLayer;
    public string pickupLayerName = "Pickable";
    public string cameraLayerName = "PickupCamera";

    [Space(10.0f)]
    public float pickMaxDistance = 1.5f;
    public float throwForce = 20.0f;

    [Space(10.0f)]
    public Transform hands;


    [Header("Keys")]
    [SearchableEnum] public KeyCode interactKey;
    [SearchableEnum] public KeyCode dropKey;

    [Header("Animation")]
    public float lerpTime = 0.3f;
    public AnimationCurve lerpCurve = AnimationCurve.Linear(0, 0, 1, 1);

    PickableObject holdee;
    Transform previousParent;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (holdee == null && Input.GetKeyDown(interactKey))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.SphereCast(ray, PickSphereRadius, out hit, pickMaxDistance, pickupLayer.value))
            {
                GameObject go = hit.transform.gameObject;
                holdee = go.GetComponent<PickableObject>();

                if (holdee)
                    StartCoroutine(MoveToHands(holdee));
            }

            return;
        }

        if (holdee != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                UseObject();
            }

            if (Input.GetKeyDown(dropKey))
            {
                DropObject();
                holdee = null;
            }

        }
    }

    void UseObject()
    {
        if (holdee.DropOnUse)
        {
            holdee.gameObject.layer = LayerMask.NameToLayer(pickupLayerName);
            AttachObjectToHand(false, holdee);
            holdee.EnablePhysics(true);
        }

        holdee.OnLeftClick(cam.transform, throwForce);

        if (holdee.DropOnUse)
        {
            holdee = null;
        }
    }

    void DropObject()
    {
        holdee.gameObject.layer = LayerMask.NameToLayer(pickupLayerName);
        AttachObjectToHand(false, holdee);
        holdee.EnablePhysics(true);
    }

    void AttachObjectToHand(bool state, PickableObject go)
    {
        if (state)
        {
            previousParent = go.transform.parent;
            go.transform.SetParent(hands);
            go.transform.localPosition = Vector3.zero;
        }
        else
        {
            go.transform.SetParent(previousParent, true);
        }
    }

    IEnumerator MoveToHands(PickableObject go)
    {
        if (go == null)
            yield break;

        Vector3 startWorld = go.transform.position;
        Quaternion startRotation = go.transform.rotation;

        go.gameObject.layer = LayerMask.NameToLayer(cameraLayerName);
        go.EnablePhysics(false);

        float t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime / lerpTime;
            float tt = lerpCurve.Evaluate(t);

            go.transform.position = Vector3.Lerp(startWorld, hands.position, tt);

            if (go.changeRotationOnPickup)
                go.transform.rotation = Quaternion.Lerp(startRotation, hands.rotation, tt);
            
            yield return null;
        }

        AttachObjectToHand(true, go);
    }
}
