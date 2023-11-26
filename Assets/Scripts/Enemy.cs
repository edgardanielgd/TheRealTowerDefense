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
    private static float DEFAULT_SPAWN_TIME = 1f;
    private Tower parent;

    // With angle, speed and reference tower dimensions
    // it should be enough to calculate this object's
    // position
    public float angle = 0;
    public float maxLife = DEFAULT_FULL_LIFE;
    public float spawnTime = DEFAULT_SPAWN_TIME;

    private float angularSpeed;
    private float life;

    // Start is called before the first frame update
    void Start()
    {
        angularSpeed = DEFAULT_ANGULAR_SPEED;
        life = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        float radiusOffset = parent.getRadiusOffset();
        float pathWidth = parent.getPathWidth();
        float towerHeight = parent.getHeight();
        float angleOffset = parent.getAngleOffset();

        Vector3 towerPos = parent.getPosition();

        // Apply transformation to next position
        float radius = (float)(
            radiusOffset - pathWidth / 2 - pathWidth * angle / (2 * Mathf.PI ) 
        );

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

        Vector3 parentPosition = parent.transform.position;
        Vector3 selfPosition = transform.position;

        // Face always the movement vector
        Vector3 toCenter = parentPosition - selfPosition;
        toCenter.y = 0;
        toCenter.Normalize();

        var centerAngle = Mathf.Acos(toCenter.x) - Mathf.PI / 2;
        toCenter.x = Mathf.Cos(centerAngle);
        toCenter.z = Mathf.Sin(centerAngle);

        Quaternion rotation = Quaternion.LookRotation(toCenter, Vector3.up);
        transform.rotation = rotation;

        // Update angle position
        float delta = Time.deltaTime;
        angle += angularSpeed * delta;
    }

    // Getters and setters for this object
    public void setParentTower( Tower tower ) { 
        this.parent = tower;
    }
    public void setAngle(  float angle ) { this.angle = angle; }

    public void OnCollisionEnter(Collision collision)
    {
        print("Collides");
    }
    public void ApplyBulletHit( String bulletType )
    {
        
        switch(bulletType)
        {
            case "Ball":
                life -= 50;
                break;

            case "Arrow":
                life -= 10;
                break;

            case "Hand":
                life -= 80;
                break;
        }

        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
