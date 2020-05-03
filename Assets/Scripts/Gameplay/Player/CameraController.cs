using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform body;
    public float mouseSensitivity = 1.2f;
    public bool invertY = false;

    public bool Locked { get => m_locked; set => m_locked = value; }
    bool m_locked = false;

    LayerMask interactionMask;
    float xAxisClamp = 0f;

    DynamicDepthOfField dynamicDepthOfField;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        interactionMask = body.GetComponent<MovementController>().interactionMask;
        dynamicDepthOfField = GetComponent<DynamicDepthOfField>();
    }


    private void Update ()
    {
        if(Time.timeScale == 0)        
            return;
        
        RotateCamera();
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * mouseSensitivity;
        float rotAmountY = mouseY * mouseSensitivity;

        xAxisClamp -= rotAmountY;

        Vector3 targetRotCamera = transform.rotation.eulerAngles;
        Vector3 targetRotBody = body.rotation.eulerAngles;

        if (invertY)
            rotAmountY *= -1;

        targetRotCamera.x -= rotAmountY;
        targetRotCamera.z = 0;

        targetRotBody.y += rotAmountX;

        if (xAxisClamp > 90)
        {
            xAxisClamp = targetRotCamera.x = 90;

        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90f;
            targetRotCamera.x = 270f;
        }

        transform.rotation = Quaternion.Euler(targetRotCamera);
        body.rotation = Quaternion.Euler(targetRotBody);

        dynamicDepthOfField.UpdateDepthOfField();
    }


    private void OnDrawGizmos()
    {
        if (dynamicDepthOfField != null)
        {
            if (dynamicDepthOfField.HasFocus)
            {
                Vector3 p = dynamicDepthOfField.FocusPoint;
                Gizmos.DrawWireSphere(p, 0.1f);
                Debug.DrawRay(transform.position, transform.forward * Vector3.Distance(transform.position, p));
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * 100f);
            }
        }
    }
}
