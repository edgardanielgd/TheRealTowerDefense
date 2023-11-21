using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Posisionate( Vector3 cameraPosition, Vector3 targetPosition)
    {
        transform.position = new Vector3(
                    cameraPosition.x,
                    cameraPosition.y,
                    cameraPosition.z
                );

        var velocity = new Vector3(
            targetPosition.x - cameraPosition.x,
            targetPosition.y - cameraPosition.y,
            targetPosition.z - cameraPosition.z
        );

        velocity.Normalize();

        velocity *= 50f;

        var arrowBody = GetComponent<Rigidbody>();

        arrowBody.velocity = velocity;

        // Get point direction where arrow is aiming to
        Quaternion rotation = Quaternion.LookRotation(velocity, Vector3.up);
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Delete this arrow
        Destroy(gameObject);

        GameObject target = collision.gameObject;

        if (target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.ApplyBulletHit("Arrow");
            }
        }
    }
}
