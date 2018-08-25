using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    // Fluid move/jump pair is 6/5.2
    [Range(4f, 10f)]
    public float movespeed;
    [Range(1f, 10f)]
    public float jumpPower;
    [Range(0f, 2f)]
    public float jumpDelay; // Optimal value is around 1.15

    private bool isGrounded;
    private bool canJump;
    private float distToGround;

    void Start ()
    {
        distToGround = this.gameObject.GetComponent<Collider>().bounds.extents.y + 0.1f;    // Get the distance to check if object is touching the ground
        canJump = true;

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
        Vector3 movement = new Vector3(x * movespeed * Time.fixedDeltaTime, 0, 0);
        
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
            if (isGrounded && canJump)
            {
                Vector3 jumpDir;
                jumpDir = (move == Vector3.zero) ? Vector3.zero : ((move.x > 0) ? Vector3.right : Vector3.left);    // Looks ugly :)
                this.gameObject.GetComponent<Rigidbody>().AddForce((jumpDir + Vector3.up) * jumpPower, ForceMode.Impulse);
                isGrounded = false;
                canJump = false;
                Invoke("ResetCanJump", jumpDelay);  // Delay the next jump by jumpDelay seconds, so player cannot spam
            }
        }
    }

    void CheckGrounded()
    {
        Vector3 pos = this.transform.position;
        float offset = this.gameObject.GetComponent<BoxCollider>().size.x / 2.0f;
        Ray r1 = new Ray(pos + new Vector3(offset, 0, 0), Vector3.down);
        Ray r2 = new Ray(pos + new Vector3(-offset, 0, 0), Vector3.down);

        isGrounded = (Physics.Raycast(r1, distToGround) || Physics.Raycast(r2, distToGround));  // Cast ray downwards to check if we are on the ground
        if (isGrounded == true)
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;  // Zero out the velocity. This solves some jumping bugs 
        }
    }

    void ResetCanJump()
    {
        canJump = true;
    }
}
