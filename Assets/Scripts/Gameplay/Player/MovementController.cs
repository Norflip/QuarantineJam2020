using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovementController : MonoBehaviour
{
    public LayerMask interactionMask;
    public float movementSpeed = 15.0f;

    [Header("Gravity")]
    public Vector3 gravityDirection = new Vector3(0, -1, 0);
    public float gravityForce = 9.0f;

    [SerializeField] 
    float velocity;

    CharacterController characterController;

    [SerializeField] private bool walking = true;
    [SerializeField] private bool grounded = true;
    [SerializeField] private Vector3 externalVelocity;

    private void Awake()
    {
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

        grounded = (characterController.Move(moveDirection * Time.fixedDeltaTime) & CollisionFlags.Below) != 0;

        externalVelocity *= 0.95f;
    }

}
