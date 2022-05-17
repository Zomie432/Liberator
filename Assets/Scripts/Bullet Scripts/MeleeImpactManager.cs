using System.Collections.Generic;
using UnityEngine;
using static BulletImpactManager;

public class MeleeImpactManager : MonoBehaviour
{
    /* size of the each particle systems object pool */
    [SerializeField] private int particleSystemBuffer = 5;

    /* list of Melee impacts */
    public List<Impact> meleeImpacts = new List<Impact>();

    /* stores the object pool related to a string, seconds string is the key to the repective object pool */
    public Dictionary<string, string> meleeImpactDictionary = new Dictionary<string, string>();

    public Dictionary<string, AudioClip> meleeImpactAudioClipDictionary = new Dictionary<string, AudioClip>();

    /* instance of this object, singleton pattern */
    private static MeleeImpactManager m_Instance;
    public static MeleeImpactManager Instance
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
    * copys all of the Melee impacts from list to the dictionary
    */

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple MeleeImpactManagers! Destroying the newest one: " + this.name);
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        foreach (Impact impact in meleeImpacts)
        {
            meleeImpactDictionary.Add(impact.objectTag, ObjectPoolManager.CreateObjectPool(impact.collisionParticleSystem, particleSystemBuffer));
            meleeImpactAudioClipDictionary.Add(impact.objectTag, impact.impactAudio);
        }
    }

    /*
     * Spawns a impact at position, with the forward direction, impact is found by @Param: objectTag
     */
    public void SpawnMeleeImpact(Vector3 position, Vector3 forward, string objectTag)
    {
        if (meleeImpactDictionary.ContainsKey(objectTag))
        {
            UpdateSpawnMeleeImpact(position, forward, ObjectPoolManager.SpawnObject(meleeImpactDictionary[objectTag]));
        }
        else
        {
            UpdateSpawnMeleeImpact(position, forward, ObjectPoolManager.SpawnObject(meleeImpactDictionary[meleeImpacts[0].objectTag]));
        }
    }

    public AudioClip GetAudioClipForImpactFromTag(string objectTag)
    {
        if (meleeImpactDictionary.ContainsKey(objectTag))
        {
            return meleeImpactAudioClipDictionary[objectTag];
        }
        else
        {
            return meleeImpactAudioClipDictionary[meleeImpacts[0].objectTag];
        }
    }

    /*
     * Updates the particle systems forward and position vectors
     */
    private void UpdateSpawnMeleeImpact(Vector3 position, Vector3 forward, PoolableObject particleSystem)
    {
        particleSystem.transform.position = position;
        particleSystem.transform.forward = forward;
    }
}
