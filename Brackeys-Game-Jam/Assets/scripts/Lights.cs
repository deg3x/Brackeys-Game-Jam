using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour {

    public GameObject lightParticleCookie;

    void Start ()
    {
        Light light = this.GetComponent<Light>();
        Color lColor = light.color;
        float angle = light.spotAngle;
        float size = 2 * (Mathf.Tan(angle) * transform.position.y);
        Debug.Log(size);
        GameObject cookie = (GameObject)Instantiate(lightParticleCookie);
        Vector3 offset = new Vector3(0, 0.2f, 0);
        cookie.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        ParticleSystem pS1 = cookie.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main1 = pS1.main;
        main1.startSize = size;
        main1.startColor = lColor;

    //    RaycastHit hit;

    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit) && hit.collider.tag == "ground")
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
    //        GameObject cookie2 = (GameObject) Instantiate(lightParticleCookie);
    //        Vector3 offset1= new Vector3(0, 0.2f, 0);
    //        cookie.transform.position = hit.point + offset1;
    //        ParticleSystem pS = cookie.GetComponent<ParticleSystem>();
    //        ParticleSystem.MainModule main = pS.main;
    //        main.startSize = size;
    //        Debug.Log(main.startSize);
    //    }
        }
	
	// Update is called once per frame
	//void Update ()
 //   {

 //       //Vector3 point = ray.origin + (ray.direction * 10);
 //       //Debug.Log("World point " + point);
 //   }

}
