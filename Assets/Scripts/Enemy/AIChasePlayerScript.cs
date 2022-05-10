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
        if (sqrDistance > agent.config.maxDistance * agent.config.maxDistance && sqrDistance > gun.bulletRange)
        {
            //constantly sets move target for enemy to the player
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }
            gun.ShootAtTarget(agent.playerTransform.position);
    }
            

    public void Exit(AIAgent agent)
    {

    }

}
