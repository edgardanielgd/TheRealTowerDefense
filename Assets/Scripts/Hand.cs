using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.XR;

public class Hand : Weapon
{

    static float DEFAULT_HAND_LIFETIME = 2f;

    public float handLifetime = DEFAULT_HAND_LIFETIME;

    private Func<bool> OnDelete;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DropHand", handLifetime);
    }

    void DropHand()
    {
        if (OnDelete != null)
            OnDelete();
    }

    public void setDelegates(Func<bool> onDelete)
    {
        OnDelete = onDelete;
    }

    public void Posisionate(Vector3 cameraPosition, Vector3 targetPosition)
    {
        // Get vector to camera pos
        Vector3 toCamera = new Vector3(
            cameraPosition.x - targetPosition.x,
            cameraPosition.y - targetPosition.y,
            cameraPosition.z - targetPosition.z
        );
        toCamera.Normalize();

        transform.position = targetPosition + toCamera * 10;

        // Rotate hand
        transform.rotation = Quaternion.LookRotation(toCamera, Vector3.left);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger");
    }

    public void DestroyHand()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        print("Hand Collision");
        if (collision != null)
        {
            GameObject target = collision.gameObject;

            if (target != null)
            {
                Enemy enemy = target.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.ApplyBulletHit(damage);
                }
            }
        }
    }
}
