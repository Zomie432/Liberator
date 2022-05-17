using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class IPickable : ISpawnable
{
    bool bJustDropped = false;

    public virtual void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    public virtual void OnPickup(GameObject picker) 
    { 
        //Debug.Log(name + " was picked up...");
        Despawn();
    }

    public virtual void OnDrop()
    {
        //Debug.Log("Just droped " + name);
        bJustDropped = true;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        //Debug.Log(name + " has collided with " + other.name);
        if (other.tag == "Player" && !bJustDropped)
            OnPickup(other.gameObject);
    }
    public void OnTriggerExit(Collider other)
    {
        bJustDropped = false;
    }
}
