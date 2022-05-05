using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashBang : BaseThrowables
{
    [SerializeField] float explodeDistanceFromPlayer = 5.0f;
    [SerializeField] Transform particleSystemSpawnTransform;

    [SerializeField] Image flashbangImage;
    [SerializeField] float flashbangTimeDamp = 0.1f;

    [SerializeField] Color flashbangImageColor;

    public override void Update()
    {
        base.Update();

        if(flashbangImageColor.a > 0f)
        {
            flashbangImageColor.a -= Time.deltaTime * flashbangTimeDamp;
            flashbangImage.color = flashbangImageColor;
        }
    }

    public override void OnThrowableExplode() 
    {
        //base.OnThrowableExplode();

        Vector3 explodeForward = fpCamera.transform.forward;
        Vector3 camPosition = fpCamera.transform.position;

        PoolableObject flashBangParticleSystem = m_ExplodeParticleSystemPool.SpawnObject();
        flashBangParticleSystem.transform.forward = explodeForward;
        flashBangParticleSystem.transform.position = camPosition + explodeForward * explodeDistanceFromPlayer;

        flashbangImageColor =  Color.white;

        Debug.DrawLine(fpCamera.transform.position, camPosition + explodeForward * explodeDistanceFromPlayer, Color.cyan, 2f);
    }
}
