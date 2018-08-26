using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCharacter : MonoBehaviour {

    #region PublicStuff
    public GameObject character;
    public float offsetZ = -10;
    public float offsetY = 8;
    #endregion PublicStuff

    void Update ()
    {
        Vector3 cameraPosition = new Vector3(character.transform.position.x, character.transform.position.y + offsetY, character.transform.position.z + offsetZ);
        this.transform.position = cameraPosition;
    }
}
