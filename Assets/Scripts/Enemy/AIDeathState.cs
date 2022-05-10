using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public Vector3 direction;
    //TO DO: Implement combat, health, and ragdoll death animation. ONce done, Death state
    //can be completed afterwards. Animators not initialized as this is just AI testing.
    public AIStateID GetId()
    {
        return AIStateID.Death;
    }

    public void Enter(AIAgent agent)
    {
        agent.ragdoll.ActivateRagdoll();
        agent.ragdoll.ApplyForce(direction * agent.config.dieForce);
        var rigidBodies = agent.GetComponentsInChildren<Rigidbody>();
        foreach(var rigidbody in rigidBodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.freezeRotation = false;
        }
        agent.gameObject.SetActive(false);
    }

    public void Update(AIAgent agent)
    {
        agent.mesh.material.SetColor("_Color", Color.black * 0);
    }

    public void Exit(AIAgent agent)
    {

    }
}
