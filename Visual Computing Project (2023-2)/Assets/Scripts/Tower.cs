using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private readonly float angleOffset = Mathf.PI;
    private readonly float radiusOffset = 15f;
    private readonly float pathWidth = 0.92f;
    private readonly float height = 17.75f;
    private readonly float laps = 7;

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
    public float getLaps() { return laps; }
}
