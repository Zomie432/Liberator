using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateID initialState;
    public NavMeshAgent navMeshAgent;
    public AIAgentConfig config;
    public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //sets the player transform to look for the player tagged object
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //GameManager.instance.playerPosition
        //creates a new state machine for this agent type. 
        stateMachine = new AIStateMachine(this);
        //adds the chase player to the enum for AIState
        stateMachine.RegisterState(new AIChasePlayerScript());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        //sets state to initial state.
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        //constantly updates the machine
        stateMachine.Update();
    }
}
