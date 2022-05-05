using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolParticleSystem : MonoBehaviour
{
    /*
     * When particle system has stoop emitted, it returns back to pool automatically
     */
    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
    }
}
