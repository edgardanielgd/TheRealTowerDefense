using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{
    // Default angular speed for enemies
    private static float DEFAULT_ANGULAR_SPEED = 0.2f;
    private static float DEFAULT_FULL_LIFE = 100f;
    private Tower parent;

    // With angle, speed and reference tower dimensions
    // it should be enough to calculate this object's
    // position
    public float angle = 0;
    private float angularSpeed;
    private float life;

    // Start is called before the first frame update
    void Start()
    {
        angularSpeed = DEFAULT_ANGULAR_SPEED;
        life = DEFAULT_FULL_LIFE;
    }

    // Update is called once per frame
    void Update()
    {
        // Apply transformation to next position
        float radius = (float)(
            parent.getRadiusOffset() - parent.getPathWidth() / 2 - parent.getPathWidth() / (2 * Mathf.PI ) * angle
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
            parent.getHeightOffset() + elevation_factor * angle
        );

        // Apply transformation to object
        transform.position = new Vector3(x, y, z);

        // Face always the movement vector
        Vector3 newRotation = new Vector3(
           transform.rotation.x,
           transform.rotation.y,
           transform.rotation.z  
        );
        // transform.rotation.SetEulerRotation(newRotation);

        // Update angle position
        float delta = Time.deltaTime;
        angle += angularSpeed * delta;
    }

    // Getters and setters for this object
    public void setParentTower( Tower tower ) { 
        this.parent = tower;

        
    }
    public void setAngle(  float angle ) { this.angle = angle; }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        print("ola");
    }
}
