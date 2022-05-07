using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxShield()
    {
        // Sets Slider max health to be whatever the players max health is
        slider.maxValue = GameManager.Instance.playerScript.GetPlayersMaxShield();
    }

    public void UpdateShieldBar()
    {
        // Updates Slider value to represent the players current health
        slider.value = GameManager.Instance.playerScript.GetCurrentPlayerShield();
    }
}
