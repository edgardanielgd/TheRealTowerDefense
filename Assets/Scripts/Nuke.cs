using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : Weapon
{
    // Start is called before the first frame update
    private static float DEFAULT_NUKE_RANGE = 10f;
    public float nukeRange = DEFAULT_NUKE_RANGE;

    void Start()
    {
        
    }

    public void Posisionate(Vector3 referencePosition)
    {
        transform.position = new Vector3(
            referencePosition.x,
            referencePosition.y + 500,
            referencePosition.z
        );
    }

    // Update is called once per frame
    void Update()
    {
        var rigidBody = GetComponent<Rigidbody>();

        rigidBody.AddForce(new Vector3(
            0, -15, 0
        ));
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Make damage
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, nukeRange);

        foreach (Collider col in hitColliders)
        {
            if (col.TryGetComponent(out Enemy enemy))
            {
                enemy.ApplyBulletHit(damage);
            }
        }

        Destroy(gameObject);
    }
}
