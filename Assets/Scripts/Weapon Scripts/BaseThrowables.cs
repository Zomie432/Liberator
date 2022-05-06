using System.Collections;
using UnityEngine;


public class BaseThrowables : BaseWeapon
{
    [SerializeField] int maxThrowablesAmount = 2;
    [SerializeField] float throwableExplodeTimer = 1.0f;

    [SerializeField] PoolableObject explodeParticleSystemPrefab;

    int m_CurrentThrowablesAmount = 1;

    protected ObjectPool m_ExplodeParticleSystemPool;

    public BaseThrowables()
    {
        allowPlayerToHoldAimTrigger = false;
        allowPlayerToHoldAttackTrigger = false;
    }

    public override void Start()
    {
        base.Start();

        m_ExplodeParticleSystemPool = new ObjectPool(explodeParticleSystemPrefab, maxThrowablesAmount);
        m_CurrentThrowablesAmount = maxThrowablesAmount;
    }

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

    public override void Attack()
    {
        if (!HasThrowablesLeft()) return;

        if (TakeAction(m_LastAttackTime, attackDelay))
        {
            GetAnimator().SetTrigger(attackAnimationTriggerName);
            UpdateLastAttackTime();

            StartCoroutine(OnThrowableExplode(fpCamera.transform.forward));
            //Invoke("OnThrowableExplode", fpCamera.transform.forward, throwableExplodeTimer);
        }
    }

    // public virtual void OnThrowableExplode(Vector3 camForward) { Debug.Log(name + " just exploded!"); }

    public virtual IEnumerator OnThrowableExplode(Vector3 camForward)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log(name + " just exploded!");
    }

    /*
     * returns if player can switch to another wepaon
    */
    public override bool CanSwitchWeapon()
    {
        return TakeAction(m_LastAttackTime, attackDelay);
    }

    /*
    * decreases throwables amount
    */
    protected void DecrementThrowablesAmount()
    {
        m_CurrentThrowablesAmount--;
    }

    /*
    * returns if players has more grenades left or not
    */
    public bool HasThrowablesLeft()
    {
        return AmountOfThrowablesLeft() > 0;
    }

    /*
    * returns max grenades player can have
    */
    public int GetMaxAmountOfThrowables()
    {
        return maxThrowablesAmount;
    }

    /*
    * returns current grenades amount
    */
    public int AmountOfThrowablesLeft()
    {
        return m_CurrentThrowablesAmount;
    }

    /*
    * returns the explode time for this throwable
    */
    public float GetExplodeTimer()
    {
        return throwableExplodeTimer;
    }
}
