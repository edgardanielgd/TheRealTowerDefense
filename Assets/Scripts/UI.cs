using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour {

    public Main main;

    private GameObject rufianeesTextObject = null;

    // Start is called before the first frame update
    void Start()
    {
        //var creditsPanel = transform.Find("CreditsPanel").gameObject;
        //rufianeesTextObject = creditsPanel.transform.Find("RufianesTitle").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //var text = rufianeesTextObject.GetComponent<TextMeshPro>();
        //text.SetText("Rufianes: " + main.GetRufianes());
        //print(main.GetRufianes());
    }

    public void OnRock()
    {
        main.LaunchRock();
    }

    public void OnFallingObject()
    {
        main.SwitchFallingObjectPowerup();
    }

    public void OnArrows()
    {
        main.SwitchArrowsPowerup();
    }

    public void OnHand()
    {
        main.SwitchHandPowerup();
    }
}
