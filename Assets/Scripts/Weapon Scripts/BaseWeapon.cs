using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
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

    [Header("Audio Settings")]

    /* audio clip played when player attacks */
    [SerializeField] AudioClip attackAudioClip;

    /* this weapons animator component */
    protected Animator m_Animator;

    /* time when weapon was last fired */
    protected float m_LastAttackTime;

    private AudioSource m_AudioSource;

    public virtual void Start()
    {
        m_LastAttackTime = attackDelay * -1;

        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
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
     * called when weapon is being switched to another weapon
     */
    public virtual void OnWeaponSwitch() { }

    /*
    * updates the last attack time
    */
    public virtual void Attack()
    {
        PlayAttackAudio();
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

    /*
    * returns is any audio is playing
    */
    protected bool IsAudioPlaying()
    {
        return m_AudioSource.isPlaying;
    }

    /*
    * plays the audio 
    */
    protected void PlayAttackAudio()
    {
        m_AudioSource.clip = attackAudioClip;
        m_AudioSource.Play();
    }

    /*
    * plays the audio 
    */
    protected void PlayAudio()
    {
        m_AudioSource.Play();
    }

    /*
    * sets the audio clip to be played next time play audio is called
    */
    protected void SetAudioClip(AudioClip clip)
    {
        m_AudioSource.clip = clip;
        m_AudioSource.time = 0;
    }

    /*
    * stops the payer audio
    */
    public void StopAudio()
    {
        m_AudioSource.Stop();
    }
}
