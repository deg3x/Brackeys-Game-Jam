using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public GameObject platform;
    public Text interactText;

    private ElevatorPlatform script;
    private bool axisInUse;

    public Animator animator;

    private void Start()
    {
        script = platform.GetComponent<ElevatorPlatform>();
        animator = FindObjectOfType<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        interactText.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        //if (animator.GetBool("isUsing"))
        //{
        //    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Use"))
        //    {
        //        animator.SetBool("isUsing", false);
        //    }
        //}

        if (Input.GetAxisRaw("Activate") != 0f)
        {
            if (!axisInUse)
            {
                script.ChangeActive();
            }

            axisInUse = true;
            if (!animator.GetBool("isUsing"))
            {
                animator.SetBool("isUsing", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactText.enabled = false;

        if (animator.GetBool("isUsing"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Use"))
            {
                animator.SetBool("isUsing", false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Activate") == 0f)
        {
            axisInUse = false;
        }
    }
}
