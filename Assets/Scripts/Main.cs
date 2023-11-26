using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System.Diagnostics;

public class Main : MonoBehaviour
{
    /* Vector represents the movement factors for camera:
     * x: Factor for radius changes
     * y: Factor for angle changes
     * z: Factor for elevation changes
     * */
    static Vector3 CAMERA_SPEED = new Vector3(0.5f, 0.03f, 1f);
    static int ARROWS_PER_BATCH = 1;
    static float DEFAULT_INITIAl_HEALTH = 100f;
    static float DEFAULT_PERSISTENT_SCORE = 1f;
    static float DEFAULT_PERSISTENT_TIME_OFFSET = 1f;
    static int DEFAULT_TIME_TO_WIN = 120;

    // Reference main Prefab elements, represent patterns that
    // are initiallizable (must be public in order to be assignable)

    // Enemies prefabs
    public Enemy[] enemiesPatterns;

    public Tower towerPattern;
    public Ball ballPattern;
    public GameObject markerPattern;
    public Arrow arrowPattern;
    public Hand handPattern;
    public Nuke nukePattern;

    public float minSpawnOffset = 1f;
    public float maxHealth = DEFAULT_INITIAl_HEALTH;
    public float persistentScore = DEFAULT_PERSISTENT_SCORE;
    public float persistentTimeOffset = DEFAULT_PERSISTENT_TIME_OFFSET;
    public int time2Win = DEFAULT_TIME_TO_WIN;

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
    private Boolean nukePowerupActive;

    // Game credits (called rufianes)
    private float rufianes;

    // Player's life
    private float health;

    // Show pointed enemy health
    private float enemyHealth;

    // Measure elapsed time
    private Stopwatch watch;

    private Tower tower;
    private GameObject marker;
    private Hand hand;

    // Custom events 
    private bool OnEnemyJourney(float damage)
    {
        health -= damage;

        if (health <= 0 )
        {
            // Game over
            GetComponent<ChangeLevel>().ChangeScene("credits");
        }

        return true;
    }

    private bool OnEnemyDeath(float income)
    {
        rufianes += income;
        return true;
    }

    private bool OnEmemyPointed(float life)
    {
        enemyHealth = life;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start playing our music
        var audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 1f;
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
        nukePowerupActive = false;
        handPowerupActive = false;

        rufianes = 100;

        health = maxHealth;

        // Start enemies spawn
        for(int i = 0; i < enemiesPatterns.Length; i ++)
        {
            StartCoroutine(SpawnEnemy(enemiesPatterns[i]));
        }

        StartCoroutine(GivePersistentScore());

        watch = Stopwatch.StartNew();
    }

    // Update is called once per frame
    void Update()
    {
        // ENEMY HEALTH

        // Reset enemy health
        enemyHealth = -1f;

        // CAMERA 

        // Update camera position
        if (Input.GetKey(KeyCode.W)) { cameraRadius -= CAMERA_SPEED.x; }
        if (Input.GetKey(KeyCode.S)) { cameraRadius += CAMERA_SPEED.x; }
        if (Input.GetKey(KeyCode.D)) { cameraAngle += CAMERA_SPEED.y; }
        if (Input.GetKey(KeyCode.A)) { cameraAngle -= CAMERA_SPEED.y; }
        if (Input.GetKey(KeyCode.E)) { cameraElevation += CAMERA_SPEED.z; }
        if (Input.GetKey(KeyCode.Q)) { cameraElevation -= CAMERA_SPEED.z; }

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
        if (fallingObjectPowerupActive || arrowsPowerupActive || handPowerupActive || nukePowerupActive)
        {
            Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            var collider = tower.GetComponent<Collider>();
            var collision = collider.Raycast(ray, out hit, 100000f);

            if (collision)
            {
                Vector3 point = hit.point;
                if (fallingObjectPowerupActive || arrowsPowerupActive || nukePowerupActive)
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

        // GAME IS WON WHEN TIME ENDS
        if ((int)(watch.ElapsedMilliseconds / 1000) >= time2Win)
        {
            // Game over
            GetComponent<ChangeLevel>().ChangeScene("credits");
        }
    
    }

    IEnumerator SpawnEnemy(Enemy enemyPattern)
    {
        while(true)
        {
            // Simulatee a random type of Enemy
            Enemy enemy = Instantiate(enemyPattern) as Enemy;
            enemy.setParentTower(tower);
            enemy.setMainCamera(targetCamera);
            enemy.setAngle(Mathf.PI / 16);
            enemy.setDelegates(OnEnemyJourney, OnEnemyDeath, OnEmemyPointed);

            // Schedule next event
            var time = -Mathf.Log(UnityEngine.Random.value) * enemyPattern.spawnTime;
            yield return new WaitForSeconds(minSpawnOffset + time);
        }
    }

    IEnumerator GivePersistentScore()
    {
        while(true)
        {
            rufianes += persistentScore;
            yield return new WaitForSeconds(persistentTimeOffset);
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

    bool OnHandDelete()
    {
        SwitchHandPowerup();
        return true;
    }
    void ResetHand()
    {
        if( hand )
        {
            hand.DestroyHand();
            hand = null;
        }
    }
    
    public void CustomOnMouseDown()
    {
        Vector3 markerPosition = marker.transform.position;
        Vector3 cameraPosition = targetCamera.transform.position;

        if (fallingObjectPowerupActive)
        {
            if (this.rufianes < ballPattern.weaponCost)
            {
                return;
            }
            else
            {
                this.rufianes -= ballPattern.weaponCost;
            }

            // Throw a rock at marker's position in XZ, high in Y
            var ball = Instantiate(ballPattern);

            ball.Posisionate(markerPosition);

        } else if (arrowsPowerupActive)
        {
            if (this.rufianes < arrowPattern.weaponCost)
            {
                return;
            }
            else
            {
                this.rufianes -= arrowPattern.weaponCost;
            }

            for (int i = 0; i < ARROWS_PER_BATCH; i++)
            {
                var arrow = Instantiate(arrowPattern);
                arrow.Posisionate( cameraPosition, markerPosition);
            }
        } else if (nukePowerupActive)
        {
            if (this.rufianes < nukePattern.weaponCost)
            {
                return;
            }
            else
            {
                this.rufianes -= nukePattern.weaponCost;
            }

            var nuke = Instantiate(nukePattern);

            nuke.Posisionate(markerPosition);
        }
    }

    public float GetRufianes()
    {
        return this.rufianes;
    }

    public float GetHealth()
    {
        return this.health;
    }

    public float GetMaxHealth()
    {
        return this.maxHealth;
    }

    public int GetTimeLeft()
    {
        // Number of seconds for the game to end
        return time2Win - (int)(watch.ElapsedMilliseconds / 1000);

    }
    public float GetPointedEnemyHealth()
    {
        return enemyHealth;
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

        marker.SetActive(this.fallingObjectPowerupActive);

        // Disable any of other powerups
        this.arrowsPowerupActive = false;
        this.handPowerupActive = false;
        this.nukePowerupActive = false;

        this.ResetHand();
    }

    public void SwitchNukePowerup()
    {
        this.nukePowerupActive = !this.nukePowerupActive;

        marker.SetActive(this.nukePowerupActive);

        // Disable any of other powerups
        this.arrowsPowerupActive = false;
        this.handPowerupActive = false;
        this.fallingObjectPowerupActive = false;

        this.ResetHand();
    }
    public void SwitchArrowsPowerup()
    {
        this.arrowsPowerupActive = !this.arrowsPowerupActive;

        marker.SetActive(this.arrowsPowerupActive);

        // Disable any of other powerups
        this.fallingObjectPowerupActive = false;
        this.handPowerupActive = false;
        this.nukePowerupActive = false;

        this.ResetHand();
    }

    public void SwitchHandPowerup()
    {
        this.handPowerupActive = !this.handPowerupActive;

        // Disable any of other powerups
        this.fallingObjectPowerupActive = false;
        this.arrowsPowerupActive = false;
        this.nukePowerupActive = false;

        // Instantiate hand object
        marker.SetActive(false);
        if (this.handPowerupActive)
        {
            if(this.rufianes < handPattern.weaponCost) { 
                return; 
            } else { this.rufianes -= handPattern.weaponCost; }

            hand = Instantiate(handPattern);
            hand.setDelegates(OnHandDelete);
        } else
            this.ResetHand();
    }
}
