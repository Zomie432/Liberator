using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnManager : MonoBehaviour
{
    [SerializeField] private List<WeaponSlot> m_WeaponSlots;

    private Dictionary<uint, Weapon> m_WeaponPerfabs = new Dictionary<uint, Weapon>();
    private Dictionary<uint, BaseWeapon> m_Weapons = new Dictionary<uint, BaseWeapon>();
    private Dictionary<uint, IPickable> m_WeaponAliases = new Dictionary<uint, IPickable>();

    /* Instance of this object, singleton pattern */
    private static WeaponSpawnManager m_WeaponSpawnManager;

    public static WeaponSpawnManager Instance
    {
        get
        {
            return m_WeaponSpawnManager;
        }

        private set
        {
            m_WeaponSpawnManager = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple WeaponSpawnManagers! Destroying the newest one: " + this.name);
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < m_WeaponSlots.Count; i++)
        {
            m_WeaponPerfabs.Add(m_WeaponSlots[i].weaponID, m_WeaponSlots[i].weapon);
        }
    }

    public BaseWeapon GetWeapon(uint weaponID, Transform weaponParent)
    {
        //Debug.Log("Gettign Weapon, id: " + weaponID);
        if (!m_WeaponPerfabs.ContainsKey(weaponID)) return null;

        if (m_Weapons.ContainsKey(weaponID))
        {
            //m_Weapons[weaponID].Spawn();
            return m_Weapons[weaponID];
        }

        BaseWeapon weapon = Instantiate(m_WeaponPerfabs[weaponID].weaponPrefab, weaponParent);
        weapon.SetWeaponID(weaponID);
        m_Weapons.Add(weaponID, weapon);

        weapon.Spawn();
        weapon.OnPickup(weaponParent.gameObject);

        //Debug.Log("Created: " + weapon.name);

        return weapon;
    }

    public IPickable GetWeaponAlias(uint weaponID)
    {
        Debug.Log("Getting Weapon alias, id: " + weaponID);
        if (!m_WeaponPerfabs.ContainsKey(weaponID)) return null;

        if (m_WeaponAliases.ContainsKey(weaponID))
        {
            m_WeaponAliases[weaponID].OnDrop();

            return m_WeaponAliases[weaponID];
        }

        IPickable obj = Instantiate(m_WeaponPerfabs[weaponID].weaponAliasPrefab);
        m_WeaponAliases.Add(weaponID, obj);

        obj.Spawn();
        obj.OnDrop();

        return obj;
    }

    [System.Serializable]
    private class WeaponSlot
    {
        public uint weaponID;
        public Weapon weapon;
    }

    [System.Serializable]
    private class Weapon
    {
        public BaseWeapon weaponPrefab;
        public IPickable weaponAliasPrefab;
    }
}
