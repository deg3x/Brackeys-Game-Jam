﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    // Fluid move/jump pair is 6/5.5
    [Range(4f, 10f)]
    public float movespeed;
    [Range(1f, 10f)]
    public float jumpPower;
    [Range(0f, 2f)]
    public float jumpDelay; // Optimal value is around 1
    [Range(1f, 3f)]
    public float fallMultiplier;
    [Range(10f, 300f)]
    public float midAirMovespeed;
    [Range(10f, 100f)]
    public float ascendSpeed;
    [Range(1f, 20f)]
    public float maxAscendSpeed;
    [Range(1f, 3f)]
    public float fallAscendFactor;
    public Text ascendText;

    private bool isGrounded;
    private bool canJump;
    private float distToGround;
    private float midAirLimit;
    private Rigidbody rb;
    private CapsuleCollider col;
    private bool canAscend;

    void Start ()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        col = this.gameObject.GetComponent<CapsuleCollider>();
        distToGround = col.bounds.extents.y + 0.1f;    // Get the distance to check if object is touching the ground
        canJump = true;
        midAirLimit = jumpPower;
        canAscend = false;

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
        else
        {
            Vector3 force = Vector3.right * movement.x * midAirMovespeed * Time.fixedDeltaTime;
            if (Mathf.Abs(force.x + rb.velocity.x) > Mathf.Abs(midAirLimit))
            {
                if( movement.x < 0 && midAirLimit > 0)
                {
                    midAirLimit = -midAirLimit;
                }
                else if(movement.x > 0 && midAirLimit < 0)
                {
                    midAirLimit = -midAirLimit;
                }
                rb.velocity = new Vector3(midAirLimit, rb.velocity.y, rb.velocity.z);
            }
            else
            {
                rb.velocity += force;
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * fallMultiplier * Time.fixedDeltaTime * Physics.gravity.y;
        }

        Jump(movement);
    }

    void Jump(Vector3 move)
    {
        if (!canAscend) // Regular jump case
        {
            if (Input.GetAxisRaw("Jump") != 0f)
            {
                CheckGrounded();
                if (isGrounded && canJump)
                {
                    Vector3 jumpDir;
                    jumpDir = (move == Vector3.zero) ? Vector3.zero : ((move.x > 0) ? Vector3.right : Vector3.left);    // Looks ugly :)
                    rb.velocity = (jumpDir + Vector3.up) * jumpPower;
                    isGrounded = false;
                    canJump = false;
                    Invoke("ResetCanJump", jumpDelay);  // Delay the next jump by jumpDelay seconds, so player cannot spam
                }
            }
        }
        else    // Ascension jump case
        {
            if (Input.GetAxisRaw("Jump") != 0f)
            {
                if ((rb.velocity.y + (ascendSpeed * Time.fixedDeltaTime)) < maxAscendSpeed)
                {
                    if (rb.velocity.y < 0f) // If falling ascend faster
                    {
                        rb.velocity += new Vector3(0f, ascendSpeed * fallAscendFactor * Time.fixedDeltaTime, 0f);
                    }
                    else
                    {
                        rb.velocity += new Vector3(0f, ascendSpeed * Time.fixedDeltaTime, 0f);
                    }
                }
            }
        }
    }

    void CheckGrounded()
    {
        Vector3 pos = this.transform.position;
        //float offset = col.size.x / 2.0f;
        float offset = col.radius;
        Ray r1 = new Ray(pos + new Vector3(offset, 0, 0), Vector3.down);
        Ray r2 = new Ray(pos + new Vector3(-offset, 0, 0), Vector3.down);

        isGrounded = (Physics.Raycast(r1, distToGround) || Physics.Raycast(r2, distToGround));  // Cast ray downwards to check if we are on the ground
        if (isGrounded == true)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0);  // Zero out the velocity. This solves some jumping bugs 
        }
    }

    void ResetCanJump()
    {
        canJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            this.transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            this.transform.SetParent(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ascension"))
        {
            ascendText.enabled = true;
            canAscend = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ascension"))
        {
            ascendText.enabled = false;
            canAscend = false;
        }
    }
}
