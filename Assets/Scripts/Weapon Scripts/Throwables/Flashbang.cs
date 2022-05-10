using System.Collections;
using UnityEngine;

public class Flashbang : BaseThrowables
{
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

    public override void OnThrowThrowable(Vector3 cameraForward, float forceMultiplier = 1f)
    {
        Debug.Log("Throw" + name);
        GetRigidbody().AddForce(cameraForward * forceMultiplier, ForceMode.Impulse);
        StartCoroutine(OnThrowableExplode());
    }
}
