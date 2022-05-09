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
        agent.mesh.updateWhenOffscreen = true;
        var material = agent.GetComponentsInChildren<Material>();
        agent.mesh.material.SetColor("_Color", Color.black);
    }

    public void Update(AIAgent agent)
    {

    }

    public void Exit(AIAgent agent)
    {

    }
}
