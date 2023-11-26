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
    private GameObject enemyHealthPanelObject = null;
    private TextMeshProUGUI enemyHealthTextObject = null;

    // Start is called before the first frame update
    void Start()
    {
        // Obtain controls references
        var panel = transform.Find("Panel").gameObject;
        var creditsPanel = panel.transform.Find("CreditsPanel").gameObject;
        var rufianesObject = creditsPanel.transform.Find("RufianesTitle").gameObject;
        rufianesTextObject = rufianesObject.GetComponent<TextMeshProUGUI>();

        var healthPanel = panel.transform.Find("HealthPanel").gameObject;
        var healthBar = healthPanel.transform.Find("HealthBar").gameObject;
        healthSliderObject = healthBar.GetComponent<Slider>();

        enemyHealthPanelObject = panel.transform.Find("EnemyHealthPanel").gameObject;
        var enemyHealthObject = enemyHealthPanelObject.transform.Find("EnemyHealthValue").gameObject;
        enemyHealthTextObject = enemyHealthObject.GetComponent<TextMeshProUGUI>();

        healthSliderObject.maxValue = main.GetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        rufianesTextObject.SetText("Rufianes: " + main.GetRufianes());
        healthSliderObject.value = main.GetHealth();

        var enemyHealth = main.GetPointedEnemyHealth();

        if (enemyHealth > 0)
        {
            enemyHealthPanelObject.SetActive(true);
            enemyHealthTextObject.SetText(enemyHealth.ToString());
        } else
        {
            enemyHealthPanelObject.SetActive(false);
        }
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

    public void OnNuke()
    {
        main.SwitchNukePowerup();
    }
}
