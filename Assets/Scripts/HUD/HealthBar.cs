using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;

    public void SetMaxHealth()
    {
        // Sets Slider max health to be whatever the players max health is
        slider.maxValue = GameManager.Instance.playerScript.GetPlayersMaxHealth();
    }    

    public void UpdateHealthBar()
    {
        // Updates Slider value to represent the players current health
        slider.value = GameManager.Instance.playerScript.GetCurrentPlayerHealth();
    }


}
