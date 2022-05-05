using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    Ragdoll ragdoll;
    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;
    }

   public void TakeDamage(float _amount)
   {
        currentHealth -= _amount;
        if(currentHealth <= 0.0f)
        {
            Die();
        }
   }

    private void Die()
    {
        ragdoll.ActivateRagdoll();
    }
}
