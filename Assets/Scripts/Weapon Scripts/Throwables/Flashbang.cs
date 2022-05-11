using System.Collections;
using UnityEngine;

public class Flashbang : BaseThrowables
{
    /*
    * Called when flashbang explodes
    *   - Returns after the explode timer                                                         
    *   - Plays both the explosion audio and the particle system
    *   - Does a sphere cast that returns all the colliders in the sphere, max 25, then checks if any of them was player or enemy, if so, flash them
    *   - Pools the object after the pool timer
    */
    public override IEnumerator OnThrowableExplode()
    {
        yield return new WaitForSeconds(GetExplodeTimer());

        PlayExplodeAudio();
        PlayExplodeSFX();

        // Do Physics.OverlapSphereNonAlloc here
        Collider[] colliders = new Collider[20];
        int collidersCount = Physics.OverlapSphereNonAlloc(transform.position, 10f, colliders);
        if (collidersCount > 0)
        {
            for (int i = 0; i < collidersCount; i++)
            {
                if (colliders[i].tag == "Player")
                {
                    colliders[i].GetComponent<Player>().FlashPlayer();
                }

                if (colliders[i].tag == "Hitbox")
                {
                    colliders[i].GetComponentInParent<AIAgent>().stateMachine.ChangeState(AIStateID.Flashed);
                }
            }
        }

        Debug.Log(name + " just exploded!");
        Invoke("Pool", GetPoolTimeAfterExplosion());
    }

    /*
    * Adds a force in the forceDirection with a scale of forceMultiplier
    */
    public override void OnThrowThrowable(Vector3 forceDirection, float forceMultiplier = 1f)
    {
        Debug.Log("Throw" + name);
        GetRigidbody().AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);
        StartCoroutine(OnThrowableExplode());
    }
}
