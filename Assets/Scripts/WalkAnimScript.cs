using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAnimScript : MonoBehaviour
{
    Animator animator;
    //bool isWalking;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
      //  isWalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        input.Normalize();

        if(input.x > 0.0f || input.x < 0.0f || input.z > 0.0f || input.z < 0.0f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {            
            animator.SetBool("isWalking", false);
        }

        //if ( Input.GetKey(KeyCode.W) ||
        //    Input.GetKey(KeyCode.A) ||
        //    Input.GetKey(KeyCode.S) ||
        //    Input.GetKey(KeyCode.D))
        //{
        //    isWalking = true;
        //}
        //else
        //{
        //    isWalking = false;
        //}

        //if (isWalking)
        //{
        //    animator.SetBool("isWalking", true);
        //}
        //else
        //{
        //    animator.SetBool("isWalking", false);
        //}

    }
}
