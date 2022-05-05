using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    Ragdoll ragdoll;
    SkinnedMeshRenderer skinnedMeshRenderer;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        foreach(var rigidBody in rigidBodies)
        {
            Hitbox hitbox = rigidBody.gameObject.AddComponent<Hitbox>();
            hitbox.health = this;
        }
    }

   public void TakeDamage(float _amount, Vector3 direction)
   {
        currentHealth -= _amount;
        if(currentHealth <= 0.0f)
        {
            Die();
        }
        blinkTimer = blinkDuration;
   }

    private void Die()
    {
        ragdoll.ActivateRagdoll();
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float blendFactor = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (blendFactor * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }
}
