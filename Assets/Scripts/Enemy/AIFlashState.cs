using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFlashState : AIState
{
    float flashTimer = 5.0f;
    public AIStateID GetId()
    {
        return AIStateID.Flashed;
    }
    public void Enter(AIAgent agent)
    {
        agent.currentState = AIStateID.Flashed;
    }
    public void Update(AIAgent agent)
    {
        
        agent.mesh.material.color = Color.yellow * 1.0f;
        flashTimer -= Time.deltaTime;
        if(flashTimer <= 0.0f)
        {
            agent.stateMachine.ChangeState(AIStateID.Idle);
        }
    }
    public void Exit(AIAgent agent)
    {
       
    }
}
