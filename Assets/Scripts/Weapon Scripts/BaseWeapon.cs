using TMPro;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
    [Header("Player Settings")]

    /* bool to keep in track if player can hold down the attack button */
    [SerializeField] protected bool allowPlayerToHoldAttackTrigger = false;

    /* bool to keep in track if player can hold down the aim button */
    [SerializeField] protected bool allowPlayerToHoldAimTrigger = false;

    [Header("Delays")]

    /* amount of time it takes to attack this weapon again in seconds */
    [SerializeField] protected float attackDelay = 1.0f;

    [Header("Camera")]

    /* main camera  */
    [SerializeField] protected Camera fpCamera;

    [Header("Animation Settings")]

    /* string reference to the trigger name set on the animation component to play attack animation */
    [SerializeField] protected string attackAnimationTriggerName = "Attack";

    [Header("Weapon Settings")]

    [SerializeField] protected int damage = 25;

    [SerializeField] Vector3 weaponPositionOffset;

    /* this weapons animator component */
    protected Animator m_Animator;

    /* time when weapon was last fired */
    protected float m_LastAttackTime;
     
    public virtual void Start() 
    {
        m_LastAttackTime = 0.0f;

        m_Animator = GetComponent<Animator>();
        transform.position += weaponPositionOffset;
    }

    public virtual void OnEnable()
    {
        if (m_Animator == null)
            m_Animator = GetComponent<Animator>();
    }

    public virtual void OnDisable()
    {

    }

    /*
    * for override purposes
    */
    public virtual void Update() { }

    /*
    * updates the last attack time
    */
    public virtual void Attack()
    {
        UpdateLastAttackTime();
    }

    /*
    * for override purposes
    */
    public virtual void Reload() { }

    /*
    * for override purposes
    */
    public virtual void StartAiming() { }

    /*
    * for override purposes
    */
    public virtual void StopAiming() { }

    /*
    * for override purposes
    */
    public virtual void UpdateAmmoGUI() { }

    public void SetAnimFloat(string name, float num)
    {
        m_Animator.SetFloat(name, num);
    }

    /*
    * updates the last attack time
    */
    public void UpdateLastAttackTime()
    {
        m_LastAttackTime = Time.time;
    }

    /*
    * returns if player can switch to another wepaon
    */
    public virtual bool CanSwitchWeapon()
    {
        return false;
    }

    /*
    * returns if the weapon is reloading
    */
    public virtual bool IsWeaponReloading()
    {
        return false;
    }

    /*
    * returns if the weapon is aimed
    */
    public virtual bool IsWeaponAimed()
    {
        return false;
    }

    /*
    * returns if player can hold down attack trigger
    */
    public bool CanPlayerHoldAttackTrigger()
    {
        return allowPlayerToHoldAttackTrigger;
    }

    /*
    * returns if player can hold down aim trigger
    */
    public bool CanPlayerHoldAimTrigger()
    {
        return allowPlayerToHoldAimTrigger;
    }

    /*
    * returns the amount of damage the weapon will do
    */
    public int GetDamage()
    {
        return damage;
    }

    /*
    * returns last attack time
    */
    public float GetLastAttackTime()
    {
        return m_LastAttackTime;
    }

    /*
    * returns this weapons animator 
    */
    public Animator GetAnimator()
    {
        return m_Animator;
    }

    /*
     * Returns if object should take action depending on delay is exceeded
     */
    protected bool TakeAction(float lastTime, float delay)
    {
        return (lastTime + delay) < Time.time;
    }

    /*
    * enables/disables gameObject
    */
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
