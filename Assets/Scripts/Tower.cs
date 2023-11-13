using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private float angleOffset = Mathf.PI;
    private float radiusOffset = 0;
    private float pathWidth = 0;
    private float height = 0;
    private float heightOffset = 5;
    private float laps = 12;

    // Start is called before the first frame update
    public void CalculateDims()
    {
        var topReference = transform.Find("ReferenceTop").transform.position;
        var borderReferenceRight = transform.Find("ReferenceBorderRight").transform.position;
        var borderReferenceLeft = transform.Find("ReferenceBorderLeft").transform.position;

        // Find radius offset by distance between both references in XY
        float radius = Mathf.Sqrt(
            Mathf.Pow( topReference.x - borderReferenceRight.x, 2) + 
            Mathf.Pow( topReference.z - borderReferenceRight.z, 2)
        );

        radiusOffset = radius;

        // Find path width with a similar approach
        float width = Mathf.Sqrt(
            Mathf.Pow( borderReferenceLeft.x - borderReferenceRight.x, 2) +
            Mathf.Pow(borderReferenceLeft.z - borderReferenceRight.z, 2)
        );

        pathWidth = width;

        // Set height attributes
        heightOffset = borderReferenceRight.y;
        height = (borderReferenceLeft.y - borderReferenceRight.y) * laps;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Getters and setters for mentioned properties
    public float getRadiusOffset() { return radiusOffset; }
    public float getAngleOffset() { return angleOffset; }
    public float getPathWidth() { return pathWidth; }
    public float getHeight() { return height; }
    public float getHeightOffset() { return heightOffset; }
    public float getLaps() { return laps; }
    
    public Vector3 getPosition()
    {
        Vector3 pos = transform.Find("ReferenceTop").transform.position;
        pos.y = transform.position.y;
        return pos;
    }

}
