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
    private static float DEFAULT_ATTACK_DAMAGE = 1f;

    // Number of rufianes you get for killing this enemy
    private static float DEFAULT_INCOME = 100f;

    private Tower parent;
    private Camera mainCamera;

    private Func<float, bool> OnJourneyCompleted;
    private Func<float, bool> OnDeath;
    private Func<float, bool> OnPointed;

    // With angle, speed and reference tower dimensions
    // it should be enough to calculate this object's
    // position
    public float angle = 0;
    public float maxLife = DEFAULT_FULL_LIFE;
    public float spawnTime = DEFAULT_SPAWN_TIME;

    public float attackDamage = DEFAULT_ATTACK_DAMAGE;
    public float income = DEFAULT_INCOME;

    public float angularSpeed = DEFAULT_ANGULAR_SPEED;

    private float life;

    // Start is called before the first frame update
    void Start()
    {
        life = maxLife;

        var audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = 0.1f;
        audioSource.Play();
        //Invoke("StopLoop", 1f);
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
            radiusOffset - pathWidth / 2 - pathWidth * angle / (2 * Mathf.PI)
        );

        float x = (float)(
            towerPos.x + radius * Math.Cos(angle + angleOffset)
        );
        float z = (float)(
            towerPos.z + radius * Math.Sin(angle + angleOffset)
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
        toCenter.Normalize();

        Quaternion rotation = Quaternion.LookRotation(toCenter, Vector3.up);
        transform.rotation = rotation * Quaternion.Euler(Vector3.up * 90);

        float maxAngle = parent.getLaps() * Mathf.PI * 2;
        if (angle >= maxAngle)
        {
            // Apply damage to player
            OnJourneyCompleted(attackDamage);
            Destroy(gameObject);
            return;

        } else
        {
            // Update angle position
            float delta = Time.deltaTime;
            angle += angularSpeed * delta;
        }

        // See if player is aiming to this enemy
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        collider = GetComponent<Collider>();
        var collision = collider.Raycast(ray, out hit, 100000f);

        if (collision)
        {
            OnPointed(life);
        }
    }

    private void StopLoop()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    // Getters and setters for this object
    public void setParentTower(Tower tower) {
        this.parent = tower;
    }
    public void setMainCamera(Camera camera)
    {
        mainCamera = camera;
    }
    public void setAngle(float angle) { this.angle = angle; }

    public void setDelegates(
        Func<float, bool> onJourney,
        Func<float, bool> onDeath,
        Func<float, bool> onPointed
    ){
        OnJourneyCompleted = onJourney;
        OnDeath = onDeath;
        OnPointed = onPointed;
    }

    public void ApplyBulletHit( float damage )
    {
        life -= damage;

        if (life <= 0)
        {
            // Give rufianes to player
            OnDeath(income);

            Destroy(gameObject);
        }
    }
}
