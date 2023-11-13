using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    // Default angular speed for enemies
    private static float DEFAULT_ANGULAR_SPEED = 0.1f;
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
        float radiusOffset = parent.getRadiusOffset();
        float pathWidth = parent.getPathWidth();
        float towerHeight = parent.getHeight();
        float angleOffset = parent.getAngleOffset();
        float towerHeightOffset = parent.getHeightOffset();
        Vector3 towerPos = parent.getPosition();

        // Apply transformation to next position
        float radius = (float)(
            radiusOffset - pathWidth / 2 - pathWidth * angle / (2 * Mathf.PI ) 
        );
        float elevation_factor = towerHeight / (float)(parent.getLaps() * 2 * Math.PI);

        float x = (float)(
            towerPos.x + radius * Math.Cos( angle + angleOffset )
        );
        float z = (float)(
            towerPos.z + radius * Math.Sin( angle + angleOffset )
        );

        Vector3 origin = new Vector3(
            x, towerPos.y + towerHeight + 10, z 
        );

        // Intercept origin with tower's mesh
        var collider = parent.GetComponent<Collider>();

        Ray ray = new Ray(origin, new Vector3(0, -1, 0));
        RaycastHit hit;

        if (collider.Raycast(ray, out hit, 100000f))
        {
            Vector3 point = hit.point;

            // Apply transformation to object
            transform.position = point;
        }

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
            print("Ola");
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }
}
