using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Weapon
{
    // Start is called before the first frame update
    private float life = 100;
    private bool isDying = false;

    void Start()
    {
    }

    public void Posisionate(Vector3 referencePosition)
    {
        transform.position = new Vector3(
            referencePosition.x,
            referencePosition.y + 100,
            referencePosition.z
        );
    }

    // Update is called once per frame
    void Update()
    {
        var rigidBody = GetComponent<Rigidbody>();

        rigidBody.AddForce(new Vector3(
            0, -10, 0
        ));
    }

    private void OnCollisionEnter(Collision collision)
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

        if (!isDying)
        {
            InvokeRepeating("ReduceLife", 0f, 1.0f);
            isDying = true;
        }
    }

    void ReduceLife()
    {
        life -= 10;

        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
