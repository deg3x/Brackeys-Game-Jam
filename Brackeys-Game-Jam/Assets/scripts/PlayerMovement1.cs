using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour {

    [HideInInspector]
    public Rigidbody rb;
    public float jumpFactor1 = 2.5f;
    public float jumpFactor2 = 2f;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (jumpFactor1 - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.W))
        {
            rb.velocity -= Vector3.up * Physics.gravity.y * (jumpFactor2 - 1) * Time.deltaTime;
        }
    }
}
