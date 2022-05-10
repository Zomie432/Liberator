using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class FlashBang : BaseThrowables
{
    /*
    * distance the flashbang explodes from the player
    */
    [SerializeField] float explodeDistanceFromPlayer = 5.0f;

    /*
    * images used to simulate a flashbang effect
    */
    [SerializeField] Image flashbangImage;

    /*
    * used to slow down or speed up how long flashbang lasts
    */
    [SerializeField] float flashbangTimeDamp = 0.25f;

    /*
    * color of the flashbang
    */
    [SerializeField] Color flashbangImageColor;

    [SerializeField] GameObject sphereCollider;

    /*
    * if player was flashed or not
    */
    bool bIsBeingFlashed = false;

    public override void Update()
    {
        base.Update();

        if (flashbangImageColor.a > 0f)
        {
            flashbangImageColor.a -= Time.deltaTime * flashbangTimeDamp;
            flashbangImage.color = flashbangImageColor;
        }
        else
        {
            bIsBeingFlashed = false;
        }
    }

    /*
    * Called when flashbang explodes
    */
    public override IEnumerator OnThrowableExplode(Vector3 camForward)
    {
        yield return new WaitForSeconds(GetExplodeTimer()); // returns to this function after some delay

        PlayExplodeAudio(); // Plays the flash bang audio

        Vector3 camPosition = fpCamera.transform.position; // gets current position of the MainCamera

        // spawns in the particle system and updates it forward and position vectors
        PoolableObject flashBangParticleSystem = m_ExplodeParticleSystemPool.SpawnObject();
        flashBangParticleSystem.transform.forward = camForward;
        flashBangParticleSystem.transform.position = camPosition + camForward * explodeDistanceFromPlayer;

        sphereCollider.transform.position = flashBangParticleSystem.transform.position;
        sphereCollider.SetActive(true);

        flashbangImageColor.a = 1;
        bIsBeingFlashed = true;

        //float dot = Vector3.Dot(flashBangParticleSystem.transform.position.normalized, fpCamera.transform.forward); // gets the angle between the cameras forward and position of where the flashbang exploded
        //if (dot > 0) // if its > 0, flash the player, if not don't
        //{
        //    flashbangImageColor.a = dot < 0.5f ? 1f * dot * 0.4f : 1f; // if the player is barely within view flash, if not completely out but barely seing the flash, scale the flash amount
        //    bIsBeingFlashed = true;
        //}
        //else
        //{
        //    bIsBeingFlashed = false;
        //}

        DecrementThrowablesAmount();

        Invoke("StopCollisionDetection", 0.25f);

        //Debug.DrawLine(fpCamera.transform.position, flashBangParticleSystem.transform.position, Color.cyan, 2f);
    }

    public void StopCollisionDetection()
    {
        sphereCollider.SetActive(false);
    }

    /*
     * Returns if player should be flashed currently -- temporary 
     */
    public bool ShouldPlayerBeFlashed()
    {
        return bIsBeingFlashed;
    }
}
