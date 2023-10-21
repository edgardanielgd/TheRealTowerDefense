using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{ 
    /* Vector represents the movement factors for camera:
     * x: Factor for radius changes
     * y: Factor for angle changes
     * z: Factor for elevation changes
     * */
    static Vector3 cameraSpeed = new Vector3(0.5f, 0.01f, 0.5f);

    // Reference main Prefab elements, represent patterns that
    // are initiallizable (must be public in order to be assignable)
    public Enemy enemy1Pattern;
    public Tower towerPattern;
    public GameObject ballPattern;

    // Target camera that rotates surrounding the target tower
    public Camera targetCamera;

    // Camera position will be calculated from cylindrical coordinates
    private float cameraRadius;
    private float cameraAngle;
    private float cameraElevation;

    // Private (non assignable variables)
    private ArrayList enemies;
    private Tower tower;
    private const int enemy_count = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Create base tower
        tower = Instantiate(towerPattern) as Tower;
        tower.transform.position = new Vector3(0,20,0);
        tower.CalculateDims();

        // Create base enemies
        enemies = new ArrayList();
        for (int i = 0; i < enemy_count; i++)
        {
            Enemy enemy = Instantiate(enemy1Pattern) as Enemy;
            enemy.setParentTower(tower);
            enemy.setAngle(-i * Mathf.PI / 8);

            enemies.Add(enemy);
        }

        // Set initial camera coords based on tower position
        cameraRadius = tower.getLaps() * tower.getPathWidth() + 100;

        cameraAngle = tower.getAngleOffset();
        cameraElevation = tower.getHeight();

        SetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        // Update camera position
        if (Input.GetKey(KeyCode.W)) { cameraRadius -= cameraSpeed.x; }
        if (Input.GetKey(KeyCode.S)) { cameraRadius += cameraSpeed.x; }
        if (Input.GetKey(KeyCode.D)) { cameraAngle += cameraSpeed.y; }
        if (Input.GetKey(KeyCode.A)) { cameraAngle -= cameraSpeed.y; }
        if (Input.GetKey(KeyCode.E)) { cameraElevation += cameraSpeed.z; }
        if (Input.GetKey(KeyCode.Q)) { cameraElevation -= cameraSpeed.z; }

        // Fix position based on constraints
        float totalRadius = tower.getLaps() * tower.getPathWidth();
        if (cameraRadius < totalRadius) { cameraRadius = totalRadius; }
        else if( cameraRadius > totalRadius + 100  ) { cameraRadius = totalRadius + 100; }

        if (cameraAngle < 0) { cameraAngle = 2 * Mathf.PI; }
        else if (cameraAngle > 2 * Mathf.PI ) { cameraAngle = 0; }

        if (cameraElevation < tower.transform.position.z + 10 ) { 
            cameraElevation = tower.transform.position.z + 10; 
        }
        else if (cameraElevation > tower.transform.position.z + tower.getHeight() * 2.0)
        {
            cameraElevation = (float)(tower.transform.position.z + tower.getHeight() * 2.0);
        }

        SetCameraPosition();
    }

    void SetCameraPosition()
    {
        // Set camera position based on cylindrical 
        float x = (float)(
            tower.transform.position.x + cameraRadius * Mathf.Cos(cameraAngle)
        );
        float z = (float)(
            tower.transform.position.z + cameraRadius * Mathf.Sin(cameraAngle)
        );
        float y = (float)(
            tower.transform.position.y + cameraElevation
        );

        targetCamera.transform.position = new Vector3( x, y, z );
        targetCamera.transform.LookAt(
            new Vector3(
                tower.transform.position.x,
                tower.transform.position.y,
                tower.transform.position.z
            )
        );
    }
        

    // Callables from outside
    public void LaunchRock()
    {
        var ball = Instantiate(ballPattern);

        // Spawn rock randomly in the tower ceil
        float angle = UnityEngine.Random.value * (2 * Mathf.PI * tower.getLaps() - 5) + (2 * Mathf.PI * 5);
        float radius = (float)(
            tower.getRadiusOffset() - tower.getPathWidth() / 2 - tower.getPathWidth() / (2 * Mathf.PI) * angle
        );

        float x = (float)(
            tower.transform.position.x + radius * Math.Cos(
                angle + tower.getAngleOffset()
            )
        );
        float z = (float)(
            tower.transform.position.z + radius * Math.Sin(
                angle + tower.getAngleOffset()
            )
        );
        float y = tower.getHeight() + 100;

        ball.transform.position = new Vector3(x,y,z);
    }
}
