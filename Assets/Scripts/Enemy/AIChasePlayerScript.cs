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
        agent.currentState = AIStateID.ChasePlayer;
    }

    public void Update(AIAgent agent)
    {
        agent.mesh.material.color = Color.white;

        //stops a lot of cost for the enemy.
        // gets the squared dist from player to enemy
        float sqrDistance = (agent.playerTransform.position - agent.transform.position).sqrMagnitude;
        agent.transform.LookAt(agent.playerTransform);
        bool inSight = agent.sensor.IsInsight(agent.playerTransform.position);

        // checks if player is insight and the distance between them is < the maxdistance the player can see before having to move
        if (inSight && sqrDistance < (agent.config.maxDistance * agent.config.maxDistance))
        {
            //Debug.Log("Attack");
            agent.stateMachine.ChangeState(AIStateID.AttackPlayer);
        }
        else
        {
            //Debug.Log("Chase");
            //constantly sets move target for enemy to the player
            agent.navMeshAgent.destination = agent.playerTransform.position;

            // if the player has left the range of the enemy, make the enemy idle
            if (sqrDistance > (agent.config.maxSightDistance * agent.config.maxSightDistance))
            {
                //Debug.Log("Idle");
                agent.stateMachine.ChangeState(AIStateID.Idle);
            }
        }


    }

    public void Exit(AIAgent agent)
    {

    }

}
