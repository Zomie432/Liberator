using UnityEngine;

public class BaseMelee : BaseWeapon
{
    /* string reference to the trigger name set on the animation component to play second attack animation */
    [SerializeField] protected string attack2AnimationTriggerName = "Attack2";

    /*
    * hides the ammo GUI upon a melee weapon equip
    */
    public override void OnEnable()
    {
        base.OnEnable();
        if(AmmoManager.Instance != null)
            AmmoManager.Instance.HideAmmoGUI();

    }

    /*
    * shows the ammo GUI upon a melee weapon unequip
    */
    public override void OnDisable()
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
    public override void Attack() { Debug.Log("attack from melee"); }

    /*
    * overriden StartAiming from BaseWeapon class to act as a secondary attack for melee weapon to use
    */
    public override void StartAiming() { }
}
