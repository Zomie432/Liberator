using UnityEngine;

public class BaseMelee : BaseWeapon
{
    /* string reference to the trigger name set on the animation component to play second attack animation */
    [SerializeField] protected string attack2AnimationTriggerName = "Attack2";

    /* seconds attack audio clip */
    [SerializeField] protected AudioClip attack2Audio;

    /*
    * hides the ammo GUI upon a melee weapon equip
    */
    public override void OnEnable()
    {
        base.OnEnable();
        AmmoManager.Instance.HideAmmoGUI();
    }

    /*
    * shows the ammo GUI upon a melee weapon unequip
    */

    public override void OnWeaponSwitch()
    {
        base.OnWeaponSwitch();
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
    public override void Attack() 
    {
        base.Attack();
    }

    /*
    * called when the attack animation reaches a certain frame
    */
    public virtual void OnAttackAnimationHitEvent()
    {
        Debug.Log("Melee Hit");
    }

    /*
    * overriden StartAiming from BaseWeapon class to act as a secondary attack for melee weapon to use
    */
    public override void StartAiming() 
    {
        PlayAttack2Audio();
    }

    /*
    * plays second attack audio
    */
    protected void PlayAttack2Audio()
    {
        SetAudioClip(attack2Audio);
        PlayAudio();
    }
}
