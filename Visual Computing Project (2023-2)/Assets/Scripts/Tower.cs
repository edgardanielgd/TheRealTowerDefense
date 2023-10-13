using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private float angleOffset = Mathf.PI;
    private float radiusOffset = 0;
    private float pathWidth = 0;
    private float height = 0;
    private float heightOffset = 0;
    private float laps = 12;

    // Start is called before the first frame update
    public void CalculateDims()
    {
        var topReference = transform.Find("ReferenceTop").position;
        var borderReferenceRight = transform.Find("ReferenceBorderRight").position;
        var borderReferenceLeft = transform.Find("ReferenceBorderLeft").position;

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
        height = topReference.y;
        heightOffset = borderReferenceLeft.y;

        print(radius);
        print(pathWidth);
        print(height);
        print(heightOffset);
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
}
