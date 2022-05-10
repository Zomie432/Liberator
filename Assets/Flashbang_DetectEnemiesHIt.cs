using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashbang_DetectEnemiesHIt : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("flashbang collider: " + other.tag);

        if (other.tag == "Hitbox")
        {
            //Debug.Log("Enemy detected on flash!");
            AIAgent agent;
            if (other.TryGetComponent<AIAgent>(out agent))
            {
                agent.stateMachine.ChangeState(AIStateID.Flashed);
            }
        }
    }
}
