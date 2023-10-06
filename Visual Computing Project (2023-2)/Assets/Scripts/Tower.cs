using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private readonly float angleOffset = Mathf.PI;
    private readonly float radiusOffset = 100f;
    private readonly float pathWidth = 7.5f;
    private readonly float height = 135f;
    private readonly float heightOffset = 4.5f;
    private readonly float laps = 13;

    // Start is called before the first frame update
    void Start()
    {
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
