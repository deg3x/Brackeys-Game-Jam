using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public GameObject platform;
    public GameObject interactText;

    private ElevatorPlatform script;
    private bool axisInUse;

    private void Start()
    {
        script = platform.GetComponent<ElevatorPlatform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        interactText.SetActive(true);
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
        interactText.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Activate") == 0f)
        {
            axisInUse = false;
        }
    }
}
