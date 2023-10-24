using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {

    public Main main;

    private bool inBomb = false;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRock()
    {
        main.LaunchRock();
    }

    public void OnBomb()
    {

        inBomb = !inBomb;

        if( inBomb)
        {
            // main.LaunchBomb();
        } else
        {
            // main.ResetMode();
        }
    }
}
