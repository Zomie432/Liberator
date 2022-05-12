using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public void SetMaxHealth()
    {
        // Sets Slider max health to be whatever the players max health is
        slider.maxValue = GameManager.Instance.playerScript.GetPlayersMaxHealth();

        fill.color = gradient.Evaluate(1f);
    }

    public void UpdateHealthBar()
    {
        // Updates Slider value to represent the players current health
        slider.value = GameManager.Instance.playerScript.GetCurrentPlayerHealth();
        //Debug.Log("Slider value = " + slider.value);
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


}
