using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Main : MonoBehaviour
{
    /* Vector represents the movement factors for camera:
     * x: Factor for radius changes
     * y: Factor for angle changes
     * z: Factor for elevation changes
     * */
    static Vector3 cameraSpeed = new Vector3(0.5f, 0.03f, 1f);

    static int arrowsCountPerBatch = 1;

    // Reference main Prefab elements, represent patterns that
    // are initiallizable (must be public in order to be assignable)

    // Enemies prefabs
    public Enemy[] enemiesPatterns;

    public Tower towerPattern;
    public Ball ballPattern;
    public GameObject markerPattern;
    public Arrow arrowPattern;
    public Hand handPattern;

    public float minSpawnOffset = 1f;

    // Target camera that rotates surrounding the target tower
    public Camera targetCamera;

    // Camera position will be calculated from cylindrical coordinates
    private float cameraRadius;
    private float cameraAngle;
    private float cameraElevation;

    // Game status boolean variables
    private Boolean fallingObjectPowerupActive;
    private Boolean arrowsPowerupActive;
    private Boolean handPowerupActive;

    // Game credits (called rufianes)
    private float rufianes;

    private Tower tower;
    private GameObject marker;
    private Hand hand;

    // Start is called before the first frame update
    void Start()
    {
        // Start playing our music
        var audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0.5f;
        audioSource.Play();

        // Create base tower
        tower = Instantiate(towerPattern) as Tower;
        tower.transform.position = new Vector3(0, 0, 0);
        tower.CalculateDims();

        // Create marker
        marker = Instantiate(markerPattern) as GameObject;

        // Set initial camera coords based on tower position
        cameraRadius = tower.getLaps() * tower.getPathWidth() + 100;

        cameraAngle = tower.getAngleOffset();
        cameraElevation = tower.getHeight();

        SetCameraPosition();

        // Set initial game status
        fallingObjectPowerupActive = false;
        arrowsPowerupActive = false;
        rufianes = 100;

        // Start enemies spawn
        for(int i = 0; i < enemiesPatterns.Length; i ++)
        {
            StartCoroutine(SpawnEnemy(enemiesPatterns[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // CAMERA 

        // Update camera position
        if (Input.GetKey(KeyCode.W)) { cameraRadius -= cameraSpeed.x; }
        if (Input.GetKey(KeyCode.S)) { cameraRadius += cameraSpeed.x; }
        if (Input.GetKey(KeyCode.D)) { cameraAngle += cameraSpeed.y; }
        if (Input.GetKey(KeyCode.A)) { cameraAngle -= cameraSpeed.y; }
        if (Input.GetKey(KeyCode.E)) { cameraElevation += cameraSpeed.z; }
        if (Input.GetKey(KeyCode.Q)) { cameraElevation -= cameraSpeed.z; }

        // Fix position based on constraints
        float totalRadius = (tower.getLaps() + 1) * tower.getPathWidth();
        if (cameraRadius < totalRadius) { cameraRadius = totalRadius; }
        else if (cameraRadius > totalRadius + 100) { cameraRadius = totalRadius + 100; }

        if (cameraAngle < 0) { cameraAngle = 2 * Mathf.PI; }
        else if (cameraAngle > 2 * Mathf.PI) { cameraAngle = 0; }

        if (cameraElevation < tower.transform.position.z + 10) {
            cameraElevation = tower.transform.position.z + 10;
        }
        else if (cameraElevation > tower.transform.position.z + tower.getHeight() * 2.0)
        {
            cameraElevation = (float)(tower.transform.position.z + tower.getHeight() * 2.0);
        }

        SetCameraPosition();


        // CUSTOM EVENTS
        if (fallingObjectPowerupActive || arrowsPowerupActive || handPowerupActive)
        {
            Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            var collider = tower.GetComponent<Collider>();
            var collision = collider.Raycast(ray, out hit, 100000f);

            if (collision)
            {
                Vector3 point = hit.point;
                if (fallingObjectPowerupActive || arrowsPowerupActive)
                {
                    point.y += 2.0f;
                    marker.transform.position = point;
                } else if (handPowerupActive)
                {
                    hand.Posisionate(targetCamera.transform.position, point);
                }

                // Detect mouse press event
                if (Input.GetMouseButtonDown(0))
                {
                    this.CustomOnMouseDown();
                }
            }
        }
    }

    IEnumerator SpawnEnemy(Enemy enemyPattern)
    {
        while(true)
        {
            // Simulatee a random type of Enemy
            Enemy enemy = Instantiate(enemyPattern) as Enemy;
            enemy.setParentTower(tower);
            enemy.setAngle(Mathf.PI / 16);

            // Schedule next event
            var time = -Mathf.Log(UnityEngine.Random.value) * enemyPattern.spawnTime;
            yield return new WaitForSeconds(minSpawnOffset + time);
        }
    }

    void SetCameraPosition()
    {
        // Set camera position based on cylindrical coords
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
                tower.transform.position.y + tower.getHeight() / 2,
                tower.transform.position.z
            )
        );
    }

    void ResetHand()
    {
        if( hand)
        {
            Destroy(hand);
            hand = null;
        }
    }
    
    public void CustomOnMouseDown()
    {
        Vector3 markerPosition = marker.transform.position;
        Vector3 cameraPosition = targetCamera.transform.position;

        if (fallingObjectPowerupActive)
        {
            // Throw a rock at marker's position in XZ, high in Y
            var ball = Instantiate(ballPattern);

            ball.transform.position = new Vector3(
                markerPosition.x,
                100,
                markerPosition.z
            );
        } else if (arrowsPowerupActive)
        {
            for (int i = 0; i < arrowsCountPerBatch; i++)
            {
                var arrow = Instantiate(arrowPattern);
                arrow.Posisionate( cameraPosition, markerPosition);
            }
        }
    }

    public float GetRufianes()
    {
        return this.rufianes;
    }

    public void SetRufianes(float rufianes )
    {
        this.rufianes = rufianes;
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

    public void SwitchFallingObjectPowerup()
    {
        this.fallingObjectPowerupActive = ! this.fallingObjectPowerupActive;

        // Disable any of other powerups
        this.arrowsPowerupActive = false;
        this.handPowerupActive = false;

        this.ResetHand();
    }

    public void SwitchArrowsPowerup()
    {
        this.arrowsPowerupActive = !this.arrowsPowerupActive;

        // Disable any of other powerups
        this.fallingObjectPowerupActive = false;
        this.handPowerupActive = false;

        this.ResetHand();
    }

    public void SwitchHandPowerup()
    {
        this.handPowerupActive = !this.handPowerupActive;

        // Disable any of other powerups
        this.fallingObjectPowerupActive = false;
        this.arrowsPowerupActive = false;

        // Instantiate hand object
        if (this.handPowerupActive)
            hand = Instantiate(handPattern);
        else
            this.ResetHand();
    }
}
