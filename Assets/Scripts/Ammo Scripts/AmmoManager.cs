using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    /* An array of ammo for starting values  */
    [SerializeField] Ammo[] ammo;

    /* Stores the ammoslot related to a ammotype */
    Dictionary<AmmoType, AmmoSlot> m_StoredAmmo = new Dictionary<AmmoType, AmmoSlot>();

    /* player to notify when ammo has changed */
    [SerializeField] Player player;

    [Header("Visuals")]

    /* UI Text to update when ammo has changed */
    TextMeshProUGUI ammoGUI;

    /* Instance of this object, singleton pattern */
    private static AmmoManager m_AmmoManager;

    public static AmmoManager Instance
    {
        get
        {
            return m_AmmoManager;
        }

        private set
        {
            m_AmmoManager = value;
        }
    }

    /*
     * copys all of the ammo from ammo array to the dictionary
     */
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Multiple AmmoManagers! Destroying the newest one: " + this.name);
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        ammoGUI = GetComponentInChildren<TextMeshProUGUI>();

        foreach(Ammo _ammo in ammo)
        {
            m_StoredAmmo.Add(_ammo.ammoType, _ammo.ammoSlot);
        }
    }

    /*
     * Increases ammo 
     * @param 'ammoType': the ammotype to be increased 
     * @param 'amount': amount to be increased by
     */
    public void IncreaseAmmo(AmmoType ammoType, int amount)
    {
        int currentAmmo = m_StoredAmmo[ammoType].ammoAmount;
        int maxAmmo = m_StoredAmmo[ammoType].ammoCapacity;

        if (currentAmmo + amount > maxAmmo)
        {
            m_StoredAmmo[ammoType].ammoAmount += (maxAmmo - currentAmmo);
        }
        else
        {
            m_StoredAmmo[ammoType].ammoAmount += amount;
        }

        player.UpdateAmmoGUI();
    }

    /*
     * updates the ammo text
     */
    public void UpdateAmmoGUI(AmmoType ammoType, int currentBullets)
    {
        ammoGUI.text = currentBullets.ToString() + "/" + GetAmmoAmount(ammoType);
    }

    /*
    * hides the ammo text
    */
    public void HideAmmoGUI()
    {
        ammoGUI.enabled = false;
    }

    /*
    * shows the ammo text
    */
    public void ShowAmmoGUI()
    {
        if(ammoGUI != null)
            ammoGUI.enabled = true;
    }

    /*
     * returns ammo thats is available
     * @param 'ammoType': the ammotype to get
     * @param 'amount': amount wanted 
     */
    public int GetAmmo(AmmoType ammoType, int amount)
    {
        int currentAmmo = m_StoredAmmo[ammoType].ammoAmount;

        if (currentAmmo <= 0) return 0;

        if(currentAmmo - amount <= 0)
        {
            m_StoredAmmo[ammoType].ammoAmount -= currentAmmo;
        }
        else
        {
            currentAmmo = amount;
            m_StoredAmmo[ammoType].ammoAmount -= amount;
        }

        return currentAmmo;
    }

    /*
     * returns ammo amount
     * @param 'ammoType': the ammotype to get
     */
    public int GetAmmoAmount(AmmoType ammoType)
    {
        return m_StoredAmmo[ammoType].ammoAmount;
    }

    /*
     * returns ammo capacity
     * @param 'ammoType': the ammotype to get
     */
    public int GetAmmoCapacity(AmmoType ammoType)
    {
        return m_StoredAmmo[ammoType].ammoCapacity;
    }

    [System.Serializable]
    private class Ammo
    {
        /* ammotype for this ammo  */
        public AmmoType ammoType;

        /* ammoslot associated with the ammotype  */
        public AmmoSlot ammoSlot;
    }

    [System.Serializable]
    private class AmmoSlot
    {
        /* current ammo amount  */
        public int ammoAmount;  

        /* max ammo capacity  */
        public int ammoCapacity;  
    }

}
