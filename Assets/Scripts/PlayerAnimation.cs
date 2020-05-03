using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        input.Normalize();

        if (input.x > 0.0f || input.x < 0.0f || input.z > 0.0f || input.z < 0.0f)
        {
            animator.Play("PlayerWalk");
           // animator.SetBool("walking", true);
        }
        else
        {
            animator.Play("PlayerIdle");
           // animator.SetBool("walking", false);
        }

    }
}
