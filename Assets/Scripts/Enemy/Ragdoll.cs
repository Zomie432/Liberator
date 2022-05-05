using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidBodies;
    // Start is called before the first frame update
    void Start()
    {
        //gets all the rigid bodies from the enemy object
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach(var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
        }
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
        }
    }

    public void ApplyForce(Vector3 force)
    {
        //applies force to all
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.AddForce(force, ForceMode.VelocityChange);
        }
    }
}
