using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlies : MonoBehaviour {

    #region working
    public bool activeFX;
    public string cameraTag = "MainCamera";
    public float offset;
    public Transform cam;
    public float distance;
    #endregion

    #region ParticleSystem
    public GameObject fireFlies;
    public ParticleSystem pS;
    public ParticleSystem.MainModule main;
    public Color color1;
    public Color color2;
    #endregion

    public void Start()
    {
        main = pS.main;
        fireFlies = pS.gameObject;
        main.startColor = color1;
        //cam = GameObject.FindGameObjectWithTag(cameraTag);
        cam = Camera.main.transform;
        CheckDistance();
    }
                        
    public void Update()
    {
        cam = Camera.main.transform;
        CheckDistance();
    }

    public void CheckDistance()
    {
        distance = Vector3.Distance(cam.position, transform.position);
        if (distance <= offset)
        {
            switch (activeFX)
            {
                case true:
                    {
                        break;
                    }
                case false:
                    {
                        activeFX = true;
                        pS.Play();
                        //pS.GetComponent<ParticleSystemRenderer>().enabled = true;
                        break;
                    }
            }
        }else
        {
            switch (activeFX)
            {
                case true:
                    {
                        activeFX = false;
                        pS.Stop();
                        //pS.GetComponent<ParticleSystemRenderer>().enabled = false;
                        break;
                    }
                case false:
                    {
                        break;
                    }
            }
        }
    }

}
