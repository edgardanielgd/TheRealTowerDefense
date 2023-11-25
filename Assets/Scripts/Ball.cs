using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    private float life = 100;
    private bool isDying = false;

    void Start()
    {
        
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
                enemy.ApplyBulletHit("Ball");
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
