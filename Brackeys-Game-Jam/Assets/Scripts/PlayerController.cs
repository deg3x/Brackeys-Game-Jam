using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
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
    public GameObject ascendText;
    [Range(1f, 10f)]
    public float animSpeedFactor;
    [Range(1f, 10f)]
    public float ascSpeedFactor;

    private bool isGrounded;
    private bool canJump;
    private float distToGround;
    private float midAirLimit;
    private Rigidbody rb;
    private CapsuleCollider col;
    private bool canAscend;
    private Transform oldParent;

    // Input variables
    private Vector3 movement;
    private bool isJumping;
    private bool jumpEnabled;
    private Animator anim;
    private float animSpeed;
    private float ascSpeed;
    private float prevPos;
    private float curPos;

    void Start ()
    {
        prevPos = 0f;
        curPos = 0f;
        anim = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        col = this.gameObject.GetComponent<CapsuleCollider>();
        distToGround = col.bounds.extents.y + 0.1f;    // Get the distance to check if object is touching the ground
        CheckGrounded();
        canJump = true;
        midAirLimit = jumpPower;
        canAscend = false;

		if (movespeed == 0f)
        {
            movespeed = 10f;
        }
	}

    private void Update()
    {
        HandleInput();
    }

    void FixedUpdate () // Player uses RigidBody so movement in FixedUpdate to be in sync with physics
    {
        HandleMovement();
    }

    void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal"); 
        movement = new Vector3(x * movespeed * Time.fixedDeltaTime, 0, 0);
        prevPos = curPos;
        curPos = movement.x;
        
        if (prevPos > curPos && curPos < 0)
        {
            this.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
        }
        else if(prevPos < curPos && curPos > 0)
        {
            this.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        if (Input.GetAxisRaw("Jump") != 0f)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
            jumpEnabled = false;
        }
    }

    void HandleMovement()
    {
        CheckGrounded();
        
        if (isGrounded)
        {
            if (movement == Vector3.zero)
            {
                animSpeed = animSpeed - 2*animSpeedFactor * Time.fixedDeltaTime < 0f ? 0f : animSpeed - 2*animSpeedFactor * Time.fixedDeltaTime;
                anim.SetFloat("(Horizontal) Speed", animSpeed);
            }
            else
            {
                this.transform.Translate(movement, Space.World);
                animSpeed = animSpeed + animSpeedFactor * Time.fixedDeltaTime > 1f ? 1f : animSpeed + animSpeedFactor * Time.fixedDeltaTime;
                anim.SetFloat("(Horizontal) Speed", animSpeed);
            }
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

        Jump();
    }

    void Jump()
    {
        if (!canAscend) // Regular jump case
        {
            if (isJumping && !jumpEnabled)
            {
                CheckGrounded();
                if (isGrounded && canJump)
                {
                    Vector3 jumpDir;
                    jumpDir = (movement == Vector3.zero) ? Vector3.zero : ((movement.x > 0) ? Vector3.right : Vector3.left);    // Looks ugly :)
                    rb.velocity = (jumpDir + Vector3.up) * jumpPower;
                    anim.SetBool("isJumping", true);
                    isGrounded = false;
                    canJump = false;
                    Invoke("ResetCanJump", jumpDelay);  // Delay the next jump by jumpDelay seconds, so player cannot spam
                    jumpEnabled = true;
                }
            }
        }
        else    // Ascension jump case
        {
            if (isJumping)
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
                    anim.SetBool("isAscending", true);
                    if (rb.velocity.y < 0)
                    {
                        ascSpeed = ascSpeed - ascSpeedFactor * Time.fixedDeltaTime > 0.5f ? ascSpeed - ascSpeedFactor * Time.fixedDeltaTime : 0.5f;
                    }
                    else
                    {
                        ascSpeed = ascSpeed + ascSpeedFactor * Time.fixedDeltaTime > 1f ? 1f : ascSpeed + ascSpeedFactor * Time.fixedDeltaTime;
                    }
                    anim.SetFloat("Ascend Height", ascSpeed);
                }
                else
                {
                    rb.velocity += new Vector3(0f, maxAscendSpeed - rb.velocity.y, 0f);
                }

                jumpEnabled = true;
            }
        }
    }

    void CheckGrounded()
    {
        Vector3 pos = this.transform.position;
        float offset = col.radius;
        Ray r1 = new Ray(pos + new Vector3(offset, 0, 0), Vector3.down);
        Ray r2 = new Ray(pos + new Vector3(-offset, 0, 0), Vector3.down);
        RaycastHit hit1 = new RaycastHit();
        RaycastHit hit2 = new RaycastHit();

        if (!isGrounded && (isGrounded = (Physics.Raycast(r1, out hit1, distToGround) || Physics.Raycast(r2, out hit2, distToGround))) == true)  // Cast ray downwards to check if we are on the ground
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isAscending", false);
            ascSpeed = 0f;
            anim.SetFloat("Ascend Height", ascSpeed);
        }
        //if (isGrounded)
            //Debug.Log(hit1);
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
            oldParent = this.transform.parent;
            this.transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            this.transform.SetParent(oldParent);
            oldParent = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ascension"))
        {
            ascendText.SetActive(true);
            canAscend = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetChild(0).transform.position.y > this.transform.position.y)
        {
            ascendText.SetActive(true);
            canAscend = true;
        }
        else
        {
            ascendText.SetActive(false);
            canAscend = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ascension"))
        {
            ascendText.SetActive(false);
            canAscend = false;
        }
    }
}
