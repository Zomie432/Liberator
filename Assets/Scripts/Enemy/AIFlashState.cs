using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFlashState : AIState
{
    float flashTimer = 5.0f;
    float currentFlashTimer = 5.0f;
    public AIStateID GetId()
    {
        return AIStateID.Flashed;        
    }
    public void Enter(AIAgent agent)
    {
        agent.currentState = AIStateID.Flashed;
        agent.navMeshAgent.isStopped = true;

        agent.isFlashed = true;

    }
    public void Update(AIAgent agent)
    {
        
        agent.mesh.material.color = Color.yellow;
        currentFlashTimer -= Time.deltaTime;
        if(currentFlashTimer <= 0.0f)
        {
            agent.stateMachine.ChangeState(AIStateID.Idle);
        }
    }
    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
        currentFlashTimer = flashTimer;

        agent.isFlashed = false;
    }
}
