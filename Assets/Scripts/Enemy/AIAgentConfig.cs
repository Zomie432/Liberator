using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    //stores public versions of variables hidden away in AIState
    public float maxDistance = 2.0f;
    public float maxSightDistance = 5.0f;
    public float dieForce = 10.0f;

    public Vector2 shootSprayRadius;
}
