using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    public AIStateID GetId()
    {
        return AIStateID.Idle;
    }

    public void Enter(AIAgent agent)
    {
        agent.currentState = AIStateID.Idle;
    }

    public void Update(AIAgent agent)
    {
        agent.mesh.material.color = Color.red * 0.5f;
        //finds the player direction and checks to see if its magnitude is outside
        //the range of the agent sight.
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        if(playerDirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }
        //gets the direction the enemy is pointing.
        Vector3 agentDirection = agent.transform.forward;

        playerDirection.Normalize();

        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if(dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
    }

    public void Exit(AIAgent agent)
    {

    }
}
