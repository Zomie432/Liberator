using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine {
    public AIState[] states;
    public AIAgent agent;
    public AIStateID currentState;

    //AIMachine Constructor.
    public AIStateMachine(AIAgent _agent)
    {
        this.agent = _agent;
        int numStates = System.Enum.GetNames(typeof(AIStateID)).Length;
        states = new AIState[numStates];
    }
        

    //sets the state equal to the given states index at its id.
    public void RegisterState(AIState state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }
    
    //returns the id state the enemy is in.
    public AIState GetState(AIStateID stateID){
        int index = (int)stateID;
        return states[index];
    }

    //return state and pass it into agent.
    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    //exits the current state and sets the current one to the new state passed in.
    public void ChangeState(AIStateID newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);

    }
}
