using UnityEngine;


public class BaseThrowables : BaseWeapon
{
    [SerializeField] int maxThrowablesAmount = 2;
    [SerializeField] float throwableExplodeTimer = 1.0f;

    [SerializeField] PoolableObject explodeParticleSystemPrefab;

    int m_CurrentThrowablesAmount;

    protected ObjectPool m_ExplodeParticleSystemPool;

    public BaseThrowables()
    {
        allowPlayerToHoldAimTrigger = false;
        allowPlayerToHoldAttackTrigger = false;

        m_CurrentThrowablesAmount = maxThrowablesAmount;
    }

    public override void Start()
    {
        base.Start();

        m_ExplodeParticleSystemPool = new ObjectPool(explodeParticleSystemPrefab, maxThrowablesAmount);
    }

    public override void Update()
    {
        base.Update();
    }

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

    public override void Attack()
    {
        if (!HasThrowablesLeft()) return;

        if (TakeAction(m_LastAttackTime, attackDelay))
        {
            GetAnimator().SetTrigger(attackAnimationTriggerName);
            UpdateLastAttackTime();

            Invoke("OnThrowableExplode", throwableExplodeTimer);
            m_CurrentThrowablesAmount--;
        }        
    }

    public virtual void OnThrowableExplode() { Debug.Log(name + " just exploded!"); }

    /*
     * returns if player can switch to another wepaon
    */
    public override bool CanSwitchWeapon()
    {
        return TakeAction(m_LastAttackTime, attackDelay);
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
}
