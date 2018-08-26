﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    [Range(0f, 0.1f)]
    public float step;
    [Range(0f, 2f)]
    public float sleepTime;
    public bool activated;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool up;
    private float sleeping;

    void Start ()
    {
        startPos = start.transform.position;
        endPos = end.transform.position;
        sleeping = 0f;

        if (this.transform.position == startPos)
        {
            up = true;
        }
        else
        {
            up = false;
        }
	}
	
	void FixedUpdate ()
    {
        if (activated)
        {
            if (up)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, endPos, step);
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, startPos, step);
            }

            if (this.transform.position == endPos)
            {
                sleeping += Time.fixedDeltaTime;
                if (sleeping > sleepTime)
                {
                    sleeping = 0f;
                    up = false;
                }
            }
            else if (this.transform.position == startPos)
            {
                sleeping += Time.fixedDeltaTime;
                if (sleeping > sleepTime)
                {
                    sleeping = 0f;
                    up = true;
                }
            }
        }
	}

    public void ChangeActive()
    {
        activated = !activated;
    }
}
