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
        agent.navMeshAgent.isStopped = true;
    }
    public void Update(AIAgent agent)
    {
        agent.mesh.material.color = Color.blue;

        bool inSight = agent.sensor.IsInsight(agent.playerTransform.position);
        if (!inSight)
        {
           // Debug.Log("Exit the shoot method");
            agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
        }

        gun.ShootAtTarget(agent.playerTransform.position, agent.config.shootSprayRadius);
    }
    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.isStopped = false;
    }
}

