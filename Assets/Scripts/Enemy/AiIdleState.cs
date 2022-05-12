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

        agent.navMeshAgent.isStopped = true;
    }

    public void Update(AIAgent agent)
    {
        agent.mesh.material.color = Color.red;

        //finds the player direction and checks to see if its magnitude is outside
        //the range of the agent sight.
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;

        //gets the direction the enemy is pointing.
        Vector3 agentDirection = agent.transform.forward;

        playerDirection.Normalize();

        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if(dotProduct > 0.0f)
        {
            if (agent.sensor.IsInsight(agent.playerTransform.position))
            {
                agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
            }
        }
    }

    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }
}
