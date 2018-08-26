using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region public stuff
    public float speed = 5f;
    public float jumpSpeed = 10f;
    [HideInInspector]
    //public Rigidbody rb;
    #endregion public stuff

    //public void Start()
    //{
    //    rb = GetComponent<Rigidbody>();
    //}

    void Update ()
    {

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.up * speed * Time.deltaTime;            
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.up * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            //rb.velocity = Vector3.right * speed;
            transform.position += transform.right * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //rb.velocity = -Vector3.right * speed;
            transform.position -= transform.right * speed * Time.deltaTime;
        }

        //if (Input.GetKey(KeyCode.W) /*&& (rb.velocity.y < 1 && rb.velocity.y >= 0)*/)
        //{
        //    rb.velocity = Vector3.up * jumpSpeed;
        //}
    }
}
