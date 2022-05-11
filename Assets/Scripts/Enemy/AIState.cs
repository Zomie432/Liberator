using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AIStateID
{
    ChasePlayer,
    AttackPlayer,
    Death,
    Idle,
    Flashed
}

public interface AIState
{
    AIStateID GetId();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);
}

