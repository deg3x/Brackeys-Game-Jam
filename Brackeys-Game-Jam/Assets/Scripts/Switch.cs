using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject platform;

    private ElevatorPlatform script;
    private bool axisInUse;

    private void Start()
    {
        script = platform.GetComponent<ElevatorPlatform>();
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

    private void Update()
    {
        if (Input.GetAxisRaw("Activate") == 0f)
        {
            axisInUse = false;
        }
    }
}
