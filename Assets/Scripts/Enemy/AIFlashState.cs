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
        
    }
    public void Update(AIAgent agent)
    {
        if (flashTimer <= 0.0f)
        {
            agent.mesh.material.color = Color.white * flashTimer;
            flashTimer -= Time.deltaTime;
        
            
        }
        else
        {
            Exit(agent);
        }
    }
    public void Exit(AIAgent agent)
    {
        agent.stateMachine.ChangeState(AIStateID.Idle);
    }
}
