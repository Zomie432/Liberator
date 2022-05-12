using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateID initialState;
    public AIStateID currentState;
    [HideInInspector]public NavMeshAgent navMeshAgent;
    [HideInInspector]public AIAgentConfig config;
    [HideInInspector]public Transform playerTransform;
    [HideInInspector]public Renderer mesh;
    [HideInInspector]public Ragdoll ragdoll;
    [HideInInspector] public AiSensor sensor;

    public bool isFlashed = false;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //creates a new state machine for this agent type. 
        stateMachine = new AIStateMachine(this);
        currentState = initialState;
        sensor = GetComponent<AiSensor>();
        //adds the chase player to the enum for AIState
        stateMachine.RegisterState(new AIChasePlayerScript());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIFlashState());
        stateMachine.RegisterState(new AIAttackPlayerState());
        //sets state to initial state.
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        //sets the player transform to look for the player tagged object
        playerTransform = GameManager.Instance.player.transform;
        //constantly updates the machine
        stateMachine.Update();
    }
}
