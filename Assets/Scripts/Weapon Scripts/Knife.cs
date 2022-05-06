using UnityEngine;

public class Knife : BaseMelee
{
    /* time taken off of attack delay to preform weapon switch */
    [SerializeField] float switchWeaponDelayTimeOffAttackDelay = 0.5f;

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
     * returns if player can switch to another wepaon
    */
    public override bool CanSwitchWeapon()
    {
        return TakeAction(m_LastAttackTime, attackDelay - switchWeaponDelayTimeOffAttackDelay);
    }
}
