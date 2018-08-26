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

    private void Start()
    {
        script = platform.GetComponent<ElevatorPlatform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        interactText.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetAxisRaw("Activate") != 0f)
        {
            if (!axisInUse)
            {
                script.ChangeActive();
            }
            axisInUse = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactText.enabled = false;
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Activate") == 0f)
        {
            axisInUse = false;
        }
    }
}
