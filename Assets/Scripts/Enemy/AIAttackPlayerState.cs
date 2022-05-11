using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackPlayerState : AIState
{
    EnemyGun gun;
    public AIStateID GetId()
    {
        return AIStateID.AttackPlayer;
    }
    public void Enter(AIAgent agent)
    {
        gun = agent.GetComponentInChildren<EnemyGun>();
        agent.mesh.material.color = Color.blue;
        agent.navMeshAgent.isStopped = true;
    }
    public void Update(AIAgent agent)
    {
        bool inSight = agent.sensor.IsInsight(agent.playerTransform.localPosition);
        if (!inSight)
        {
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }
        gun.ShootAtTarget(agent.playerTransform.position);
    }
    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }
}

