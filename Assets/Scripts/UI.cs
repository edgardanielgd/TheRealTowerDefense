using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour {

    public Main main;

    private bool inBomb = false;
    private bool inArrows = false;
    private GameObject rufiantesTextObject = null;

    // Start is called before the first frame update
    void Start()
    {
        var creditsPanel = transform.Find("CreditsPanel").gameObject;
        rufiantesTextObject = creditsPanel.transform.Find("RufianesTitle").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var text = rufiantesTextObject.GetComponent<TextMeshPro>();
        text.SetText("Rufianes: " + main.GetRufianes());
        print(main.GetRufianes());
    }

    public void OnRock()
    {
        main.LaunchRock();
    }

    public void OnFallingObject()
    {

        inBomb = !inBomb;

        if(inBomb)
        {
           main.StartFallingObjectPowerup();
           main.StopArrowsPowerup();
        } else
        {
           main.StopFallingObjectPowerup();
        }
    }

    public void OnArrows()
    {

        inArrows = !inArrows;

        if (inArrows)
        {
            main.StartArrowsPowerup();
            main.StopFallingObjectPowerup();
        }
        else
        {
            main.StopArrowsPowerup();
        }
    }

    public void OnMouseDown()
    {
        print("ola de mar 2");
    }
}
