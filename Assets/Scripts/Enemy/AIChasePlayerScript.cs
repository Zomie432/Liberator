using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChasePlayerScript : AIState
{
    EnemyGun gun;

    public AIStateID GetId()
    {
        return AIStateID.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {
        gun = agent.GetComponentInChildren<EnemyGun>();
        agent.mesh.material.color = Color.red * 0.5f;
        agent.currentState = AIStateID.ChasePlayer;
    }

    public void Update(AIAgent agent)
    {
        //stops a lot of cost for the enemy.
        float sqrDistance = (agent.playerTransform.position - agent.navMeshAgent.destination).sqrMagnitude;
        agent.transform.LookAt(agent.playerTransform);  
        bool inSight = agent.sensor.IsInsight(agent.playerTransform.localPosition);
        if (sqrDistance > gun.bulletRange || !inSight)
        {
            Debug.Log("Chasing");
            //constantly sets move target for enemy to the player
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }
        else
        {
            Debug.Log("Entered the shoot method");
            //agent.navMeshAgent.isStopped = true;
            agent.stateMachine.ChangeState(AIStateID.AttackPlayer);
        }
        
    }

    public void Exit(AIAgent agent)
    {

    }

}
