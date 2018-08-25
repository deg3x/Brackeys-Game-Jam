using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    // Fluid move/jump pair is 6/5
    [Range(4f, 10f)]
    public float movespeed;
    [Range(1f, 10f)]
    public float jumpPower;

    private bool isGrounded;
    private float distToGround;

    void Start ()
    {
        distToGround = this.gameObject.GetComponent<Collider>().bounds.extents.y + 0.1f;    // Get the distance to check if object is touching the ground

		if (movespeed == 0f)
        {
            movespeed = 10f;
        }

        CheckGrounded();
	}
	
	void FixedUpdate () // Player uses RigidBody so movement in FixedUpdate to be in sync with physics
    {
        HandleMovement();
	}

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(-x * movespeed * Time.deltaTime, 0, 0);
        
        CheckGrounded();

        if (isGrounded)
        {
            this.transform.Translate(movement);
        }

        Jump(movement);
    }

    void Jump(Vector3 move)
    {
        if(Input.GetAxisRaw("Jump") != 0f)
        {
            CheckGrounded();
            if (isGrounded)
            {
                Vector3 jumpDir;
                jumpDir = (move == Vector3.zero) ? Vector3.zero : ((move.x > 0) ? Vector3.right : Vector3.left);    // Looks ugly :)
                this.gameObject.GetComponent<Rigidbody>().AddForce((jumpDir + Vector3.up) * jumpPower, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    void CheckGrounded()
    {
        Ray r = new Ray(this.transform.position, Vector3.down);

        isGrounded = Physics.Raycast(r, distToGround);  // Cast ray downwards to check if we are on the ground
        if (isGrounded == true)
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;  // Zero out the velocity. This solves some jumping bugs 
        }
    }
}
