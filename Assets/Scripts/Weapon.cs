using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    private static float DEFAULT_WEAPON_COST = 10f;
    private static float DEFAULT_DAMAGE = 10f;

    public float weaponCost = DEFAULT_WEAPON_COST;
    public float damage = DEFAULT_DAMAGE;

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent(out AudioSource audioSource)) {
            audioSource.loop = false;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {   
    }
}
