using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFlashState : AIState
{
    float flashTimer = 1.0f;
    public AIStateID GetId()
    {
        return AIStateID.Flashed;
    }
    public void Enter(AIAgent agent)
    { 
        agent.mesh.material.color = Color.white;
    }
    public void Update(AIAgent agent)
    {
        flashTimer -= Time.deltaTime;
        if (flashTimer <= 0.0f)
        {
            agent.mesh.material.color = Color.red;
            agent.stateMachine.ChangeState(AIStateID.Idle);
        }
    }
    public void Exit(AIAgent agent)
    {

    }
}
