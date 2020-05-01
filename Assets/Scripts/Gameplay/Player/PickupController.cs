using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    const float PickSphereRadius = 0.3f;

    public LayerMask pickupLayer;
    public string cameraPickup;
    public float pickMaxDistance = 1.5f;

    public Transform hands;
    public PickableObject holdee;

    [Header("Animation")]
    public float lerpTime = 0.3f;
    public AnimationCurve lerpCurve = AnimationCurve.Linear(0, 0, 1, 1);

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;    
    }

    private void Update()
    {
        if(holdee == null && Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.SphereCast(ray, PickSphereRadius, out hit, pickMaxDistance, pickupLayer.value))
            {
                GameObject go = hit.transform.gameObject;
                StartCoroutine(MoveToHands(go));
            }
        }

        if(holdee != null && Input.GetKeyDown(KeyCode.Q))
        {
            GameObject g = holdee.gameObject;
            Destroy(g);
            holdee = null;
        }
    }

    IEnumerator MoveToHands (GameObject go)
    {
        float t = 0.0f;
        Vector3 startWorld = go.transform.position;
        go.layer = LayerMask.GetMask(cameraPickup);

        Debug.Log(go.layer);
        while (t <= 1.0f)
        {
            t += Time.deltaTime / lerpTime;
            float tt = lerpCurve.Evaluate(t);

            go.transform.position = Vector3.Lerp(startWorld, hands.position, tt);
            yield return null;
        }

        go.transform.SetParent(hands);
        go.transform.localPosition = Vector3.zero;
    }
}
