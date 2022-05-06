using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashBang : BaseThrowables
{
    [SerializeField] float explodeDistanceFromPlayer = 5.0f;
    [SerializeField] Transform particleSystemSpawnTransform;

    [SerializeField] Image flashbangImage;
    [SerializeField] float flashbangTimeDamp = 0.25f;

    [SerializeField] Color flashbangImageColor;

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

    public override IEnumerator OnThrowableExplode(Vector3 camForward)
    {
        yield return new WaitForSeconds(GetExplodeTimer());

        Vector3 camPosition = fpCamera.transform.position;

        PoolableObject flashBangParticleSystem = m_ExplodeParticleSystemPool.SpawnObject();
        flashBangParticleSystem.transform.forward = camForward;
        flashBangParticleSystem.transform.position = camPosition + camForward * explodeDistanceFromPlayer;

        float dot = Vector3.Dot(flashBangParticleSystem.transform.position.normalized, fpCamera.transform.forward); // gets the angle between the cameras forward and position of where the flashbang exploded
        if (dot > 0) // if its > 0, flash the player, if not don't
        {
            flashbangImageColor.a = dot < 0.5f ? 1f * dot : 1f; // if the player is barely within view flash, if not completely out but barely seing the flash, scale the flash amount
            bIsBeingFlashed = true;
        }
        else
        {
            bIsBeingFlashed = false;
        }

        DecrementThrowablesAmount();

        Debug.DrawLine(fpCamera.transform.position, flashBangParticleSystem.transform.position, Color.cyan, 2f);
    }

    /*
     * Returns if player should be flashed currently -- temporary 
     */
    public bool ShouldPlayerBeFlashed()
    {
        return bIsBeingFlashed;
    }
}
