using UnityEngine;

public class BaseMelee : BaseWeapon
{
    /* seconds attack audio clip */
    [SerializeField] protected AudioClip attack2Audio;

    public override void Spawn() { }

    public override void Respawn() { }

    public override void OnWeaponEquip()
    {
        AmmoManager.Instance.HideAmmoGUI();
    }

    public override void OnWeaponUnequip()
    {
        AmmoManager.Instance.ShowAmmoGUI();
    }

    /*
    * doesn't allow played to hold down triggers to trigger attacks
    */
    public override void Start()
    {
        base.Start();

        allowPlayerToHoldAimTrigger = false;
        allowPlayerToHoldAttackTrigger = false;
    }

    /*
    * overriden attack from BaseWeapon class
    */
    public override void StartAttacking() 
    {
        base.StartAttacking();
    }

    public override void StopAttacking()
    {
        base.StopAttacking();
    }

    /*
    * called when the attack animation reaches a certain frame
    */
    public virtual void OnAnimationEvent_AttackHit()
    {
        Debug.Log("Melee Hit");
    }

    /*
    * overriden StartAiming from BaseWeapon class to act as a secondary attack for melee weapon to use
    */
    public override void StartAiming() 
    {
        base.StopAiming();

        bIsAttacking = true;

        PlayAttack2Audio();
    }

    public override void StopAiming()
    {
        base.StopAiming();

        bIsAttacking = false;
    }

    /*
    * plays second attack audio
    */
    protected void PlayAttack2Audio()
    {
        PlayAudioOneShot(attack2Audio);
    }

    public override bool CanSwitchWeapon()
    {
        return !bIsAttacking;
    }
}
