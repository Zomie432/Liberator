using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChasePlayerScript : AIState
{
    EnemyGun gun;
    float timer = 0.0f;

    public AIStateID GetId()
    {
        return AIStateID.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {

    }

    public void Update(AIAgent agent)
    {
        //if timer is less than 0, run the check for if the distance is greater than the range
        //from the player. If so, move the enemy to the players transform. Checked every second.
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            //stops a lot of cost for the enemy.
            float sqrDistance = (agent.playerTransform.position - agent.navMeshAgent.destination).sqrMagnitude;
            if (sqrDistance > agent.config.maxDistance * agent.config.maxDistance)
            {
                //constantly sets move target for enemy to the player
                agent.navMeshAgent.destination = agent.playerTransform.position;
            }
            timer = agent.config.maxTime;
        }
        if((agent.playerTransform.position - agent.transform.position).magnitude.abs <= gun.bulletRange)
        {
            gun.Shoot(agent.transform.forward);
        }
    }

    public void Exit(AIAgent agent)
    {

    }

}
