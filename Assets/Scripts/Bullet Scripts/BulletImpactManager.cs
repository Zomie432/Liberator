using System.Collections.Generic;
using UnityEngine;

public class BulletImpactManager : MonoBehaviour
{
    /* size of the each particle systems object pool */
    [SerializeField] private int particleSystemBuffer = 5;

    /* list of bullet impacts */
    public List<BulletImpact> bulletImpacts = new List<BulletImpact>();

    /* stores the object pool related to a string */
    public Dictionary<string, ObjectPool> bulletImpactDictionary = new Dictionary<string, ObjectPool>();

    /* instance of this object, singleton pattern */
    private static BulletImpactManager m_Instance;
    public static BulletImpactManager Instance
    {
        get
        {
            return m_Instance;
        }
        private set
        {
            m_Instance = value;
        }
    }

    /*
     * copys all of the bullet impacts from list to the dictionary
     */

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple BulletImpactManagers! Destroying the newest one: " + this.name);
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        foreach (BulletImpact impact in bulletImpacts)
        {
            bulletImpactDictionary.Add(impact.objectTag, new ObjectPool(impact.collisionParticleSystem, particleSystemBuffer));
        }
    }

    /*
     * Spawns a impact at position, with the forward direction, impact is found by @Param: objectTag
     */
    public void SpawnBulletImpact(Vector3 position, Vector3 forward, string objectTag)
    {
        if (bulletImpactDictionary.ContainsKey(objectTag))
        {
            UpdateSpawnBulletImpact(position, forward, bulletImpactDictionary[objectTag].SpawnObject());
        }
        else
        {
            UpdateSpawnBulletImpact(position, forward, bulletImpactDictionary[bulletImpacts[0].objectTag].SpawnObject());
        }
    }

    /*
     * Updates the particle systems forward and position vectors
     */
    private void UpdateSpawnBulletImpact(Vector3 position, Vector3 forward, PoolableObject particleSystem)
    {
        particleSystem.transform.position = position;
        particleSystem.transform.forward = forward;
    }


    /*
     * Stores the tag and the particle system associated with the tag
     */
    [System.Serializable]
    public class BulletImpact
    {
        public string objectTag;
        public PoolableObject collisionParticleSystem;
    }
}