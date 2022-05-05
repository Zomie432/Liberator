using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    //TO DO: Implement combat, health, and ragdoll death animation. ONce done, Death state
    //can be completed afterwards. Animators not initialized as this is just AI testing.
    public AIStateID GetId()
    {
        return AIStateID.Death;
    }

    public void Enter(AIAgent agent)
    {

    }

    public void Update(AIAgent agent)
    {

    }

    public void Exit(AIAgent agent)
    {

    }
}
