using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Reference main Prefab element
    public GameObject towerObj;

    private GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate at position (0, 0, 0) and zero rotation.
        currentTarget = Instantiate(towerObj, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.W)) {
            currentTarget.transform.position += new Vector3(0, -1, 0);
        } else if (Input.GetKeyUp(KeyCode.S))
        {
            currentTarget.transform.position += new Vector3(0, 1, 0);
        } else if (Input.GetKeyUp(KeyCode.D))
        {
            currentTarget.transform.position += new Vector3(1, 0, 0);
        } else if (Input.GetKeyUp(KeyCode.A))
        {
            currentTarget.transform.position += new Vector3(-1, 0, 0);
        }
    }
}
