using TMPro;
using UnityEngine;

public class AmmoTextManager : MonoBehaviour
{
    [Header("Visuals")]

    /* UI Text to update when ammo has changed */
    [SerializeField] TextMeshProUGUI ammoGUI;

    private static AmmoTextManager m_AmmoTextManager;
    public static AmmoTextManager Instance
    {
        get
        {
            return m_AmmoTextManager;
        }

        private set
        {
            m_AmmoTextManager = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple AmmoManagers! Destroying the newest one: " + this.name);
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateAmmoText(int currentBullets, int maxBullets)
    {
        ammoGUI.text = currentBullets.ToString() + "/" + maxBullets.ToString();
    }
}
