using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovementController : MonoBehaviour
{
    const float BODY_HIT_FORCE = 4.0f;

    public LayerMask interactionMask;
    public float movementSpeed = 15.0f;

    public string footstepSoundKey = "slippers";
    public float footstepLength = 0.2f;

    [Header("Gravity")]
    public Vector3 gravityDirection = new Vector3(0, -1, 0);
    public float gravityForce = 9.0f;

    [SerializeField] 
    float velocity;

    CharacterController characterController;

    [SerializeField] private bool walking = true;
    [SerializeField] private Vector3 externalVelocity;
    Vector3 lastPos;
    float distanceWalked;

    private void Awake()
    {
        lastPos = transform.position;
        characterController = GetComponent<CharacterController>();
        externalVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
            return;

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        input.Normalize();

        externalVelocity += gravityDirection * gravityForce * Time.fixedDeltaTime;

        Vector3 moveDirection;
        moveDirection = new Vector3(input.x, -0.1f, input.z);
        moveDirection = transform.TransformDirection(moveDirection) * movementSpeed + externalVelocity;

        characterController.Move(moveDirection * Time.fixedDeltaTime);
        externalVelocity *= 0.95f;

        distanceWalked += (transform.position - lastPos).magnitude;

        if (distanceWalked > footstepLength)
        {
            AudioManager.Instance.PlayGroup(footstepSoundKey, 1.0f);
            distanceWalked = 0.0f;
        }

        lastPos = transform.position;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.rigidbody != null)
        {
            Vector3 direction = (hit.point - transform.position).normalized;
            hit.rigidbody.AddForce(direction * BODY_HIT_FORCE, ForceMode.Force);
        }
    }
}
