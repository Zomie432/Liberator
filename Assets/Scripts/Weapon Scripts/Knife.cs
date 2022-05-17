using UnityEngine;

public class Knife : BaseMelee
{
    [SerializeField] float knifeHitRange = 0.5f;

    /*
    * triggers first attack 
    */
    public override void StartAttacking()
    {
        if (!bIsAttacking)
        {
            PlayAttackAudio();
            GetAnimator().Play("Attack1");
            UpdateLastAttackTime();
        }
    }

    public override void StopAttacking()
    {
        base.StopAttacking();
    }

    /*
    * triggers second attack 
    */
    public override void StartAiming()
    {
        if (!bIsAttacking)
        {
            PlayAttack2Audio();
            GetAnimator().Play("Attack2");
            UpdateLastAttackTime();
        }
    }

    public override void StopAiming()
    {
        base.StopAiming();
    }

    /*
    * called when the attack animation reaches a certain frame
    */
    public override void OnAnimationEvent_AttackHit()
    {
        RaycastHit hitInfo;
        //Debug.DrawLine(fpCamera.transform.position, fpCamera.transform.position + fpCamera.transform.forward * knifeHitRange, Color.red, 2f);
        if (Physics.Raycast(GameManager.Instance.mainCamera.transform.position, GameManager.Instance.mainCamera.transform.forward, out hitInfo, knifeHitRange))
        {

            if (hitInfo.collider.tag == "Hitbox")
                hitInfo.collider.GetComponent<Hitbox>().OnRaycastHit(this, transform.forward);

            MeleeImpactManager.Instance.SpawnMeleeImpact(hitInfo.point, hitInfo.normal, hitInfo.collider.tag);

            // Audio
            PlayAudioOneShot(MeleeImpactManager.Instance.GetAudioClipForImpactFromTag(hitInfo.collider.tag));
        }
    }

    /*
     * returns if player can switch to another wepaon
    */
    public override bool CanSwitchWeapon()
    {
        return !IsAttacking();
    }
}
