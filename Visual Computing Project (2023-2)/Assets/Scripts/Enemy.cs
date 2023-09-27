using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Default angular speed for enemies
    private static float DEFAULT_ANGULAR_SPEED = 0.4f;
    private Tower parent;

    // With angle, speed and reference tower dimensions
    // it should be enough to calculate this object's
    // position
    public float angle;
    private float angularSpeed;

    // Start is called before the first frame update
    void Start()
    {
        angle = 0;
        angularSpeed = DEFAULT_ANGULAR_SPEED;
    }

    // Update is called once per frame
    void Update()
    {
        // print("Starts update");
        // Apply transformation to next position
        float radius = (float)(
            parent.getRadiusOffset() - 
            (parent.getPathWidth() / Mathf.PI) * angle
        );
        float elevation_factor = parent.getHeight() / (float)(parent.getLaps() * 2 * Math.PI);

        float x = (float)(
            parent.transform.position.x + radius * Math.Cos(
                angle + parent.getAngleOffset()
            )
        );
        float z = (float)(
            parent.transform.position.z + radius * Math.Sin(
                angle + parent.getAngleOffset()
            )
        );
        float y = (float)(
            parent.transform.position.y + elevation_factor * angle
        );

        // Apply transformation to object
        transform.position = new Vector3(x, y, z);

        // TODO: Face always the movement position

        // Update angle position
        float delta = Time.deltaTime;
        angle += angularSpeed * delta;
    }

    // Getters and setters for this object
    public void setParentTower( Tower tower ) { this.parent = tower; }
}
