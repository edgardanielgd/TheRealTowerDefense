using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.XR;

public class Hand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Posisionate(Vector3 cameraPosition, Vector3 targetPosition)
    {
        // Get vector to camera pos
        Vector3 toCamera = new Vector3(
            cameraPosition.x - targetPosition.x,
            cameraPosition.y - targetPosition.y,
            cameraPosition.z - targetPosition.z
        );
        toCamera.Normalize();

        transform.position = targetPosition + toCamera * 10;

        // Rotate hand
        transform.rotation = Quaternion.LookRotation(toCamera, Vector3.left);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
