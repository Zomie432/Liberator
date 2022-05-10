using UnityEngine;

public class Knife : BaseMelee
{
    /* time taken off of attack delay to preform weapon switch */
    [SerializeField] float switchWeaponDelayTimeOffAttackDelay = 0.5f;

    [SerializeField] float knifeHitRange = 0.5f;

    /*
    * triggers first attack 
    */
    public override void Attack()
    {
        if (TakeAction(m_LastAttackTime, attackDelay))
        {
            PlayAttackAudio();
            GetAnimator().SetTrigger(attackAnimationTriggerName);
            UpdateLastAttackTime();
        }
    }

    /*
    * triggers second attack 
    */
    public override void StartAiming()
    {
        if (TakeAction(m_LastAttackTime, attackDelay))
        {
            PlayAttack2Audio();
            GetAnimator().SetTrigger(attack2AnimationTriggerName);
            UpdateLastAttackTime();
        }
    }

    /*
    * called when the attack animation reaches a certain frame
    */
    public override void OnAttackAnimationHitEvent()
    {
        RaycastHit hitInfo;
        Debug.DrawLine(fpCamera.transform.position, fpCamera.transform.position + fpCamera.transform.forward * knifeHitRange, Color.red, 2f);
        if (Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out hitInfo, knifeHitRange))
        {

            if (hitInfo.collider.tag == "Hitbox")
                hitInfo.collider.GetComponent<Hitbox>().OnRaycastHit(this, transform.forward);

            MeleeImpactManager.Instance.SpawnMeleeImpact(hitInfo.point, hitInfo.normal, hitInfo.collider.tag);

            // Audio
            SetAudioClip(MeleeImpactManager.Instance.GetAudioClipForImpactFromTag(hitInfo.collider.tag));
            PlayAudio();
        }
    }

    /*
     * returns if player can switch to another wepaon
    */
    public override bool CanSwitchWeapon()
    {
        return TakeAction(m_LastAttackTime, attackDelay - switchWeaponDelayTimeOffAttackDelay);
    }
}
