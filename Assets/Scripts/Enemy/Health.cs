using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    //[HideInInspector]
    public float currentHealth;
    AIAgent agent;
    [SerializeField] Renderer skinnedMeshRenderer;
    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Health Start");
        agent = GetComponent<AIAgent>();
        currentHealth = maxHealth;
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidBody in rigidBodies)
        {
            Hitbox hitbox = rigidBody.gameObject.AddComponent<Hitbox>();
            hitbox.health = this;
        }
        
    }

   public void TakeDamage(float _amount, Vector3 direction)
   {
        currentHealth -= _amount;
        

        if (currentHealth < 1f)
        {
            agent.stateMachine.ChangeState(AIStateID.Death);
        }
        else if(currentHealth > 0.0f && !agent.isFlashed)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
        blinkTimer = blinkDuration;
        
    }

    private void Die(Vector3 direction)
    {
        //changes agent into death state when the agent is dead.
        AIDeathState deathState = agent.stateMachine.GetState(AIStateID.Death) as AIDeathState;
        deathState.direction = direction;
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float blendFactor = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (blendFactor * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }
}
