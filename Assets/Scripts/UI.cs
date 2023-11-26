using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Main main;

    private TextMeshProUGUI rufianesTextObject = null;
    private Slider healthSliderObject = null;

    // Start is called before the first frame update
    void Start()
    {
        // Obtain controls references
        var panel = transform.Find("Panel").gameObject;
        var creditsPanel = panel.transform.Find("CreditsPanel").gameObject;
        var rufianesbject = creditsPanel.transform.Find("RufianesTitle").gameObject;
        rufianesTextObject = rufianesbject.GetComponent<TextMeshProUGUI>();

        var healthPanel = panel.transform.Find("HealthPanel").gameObject;
        var healthBar = healthPanel.transform.Find("HealthBar").gameObject;
        healthSliderObject = healthBar.GetComponent<Slider>();

        healthSliderObject.maxValue = main.GetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        rufianesTextObject.SetText("Rufianes: " + main.GetRufianes());
        healthSliderObject.value = main.GetHealth();
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
