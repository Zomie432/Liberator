using UnityEngine;

[System.Serializable]
public class BaseGun : BaseWeapon
{
    [SerializeField] AudioClip reloadAudioClip;

    [Header("Animation Settings")]

    /* string reference to the trigger name set on the animation component to play reload animation */
    [SerializeField] protected string reloadAnimationTriggerName = "Reload";

    /* string reference to the bool name set on the animation component to play ads animation */
    [SerializeField] protected string aimDownSightAnimationBoolName = "isAiming";

    [Header("Delays")]

    /* amount of time it takes to reload this weapon in seconds */
    [SerializeField] float reloadDelay = 1.0f;

    [Header("Visuals")]

    /* Muzzle flash VFX played when weapon has been fired */
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("Bullet")]

    #region Bullet Stuff
    /* prefab of the bullet that will be spawned in */
    [SerializeField] Bullet bulletPrefab;

    /* location where the bullet will be spawned */
    [SerializeField] Transform bulletSpawnLocation;

    /* the max range a bullet can travel before getting destroyed */
    [SerializeField] float bulletRange = 100f;

    /* type of ammo this gun uses */
    [SerializeField] AmmoType ammoType;

    /* max number of bullets this gun can have at one time */
    [SerializeField] int maxNumOfBullets = 30;

    [SerializeField] AudioClip bulletDropAudioClip;

    /* amount of seconds to wait to play bullet drop audio after shooting */
    [SerializeField] float bulletDropAudioInterval = 0.5f;

    /* current number of bullets */
    int m_CurrentNumOfBullets;
    #endregion

    [Header("Reload Settings")]

    [SerializeField]
    [Tooltip("Amount of seconds that get deducted from the reload delay to update the ammo GUI")]
    float reloadTimeDeductionForAmmoGUIUpdate = 0.35f;

    /* offset applied only when weapon was shot while aiming, makes the bullet look like it came out from weapon */
    [SerializeField] Vector3 aimingBulletSpawnOffset;

    /* bool to keep in track if weapon is being reloaded */
    protected bool bIsReloading;

    /* bool to keep in track if weapon is aimed or not */
    protected bool bIsAiming;

    /* time when weapon reload started */
    float m_ReloadStartTime;

    /* a objectpool that will keep track of spawned bullets automatically */
    ObjectPool m_BulletPool;

    public BaseGun()
    {

    }

    /*
    * Creates the bullet pool 
    */
    public override void Start()
    {
        base.Start();

        m_CurrentNumOfBullets = maxNumOfBullets;

        UpdateAmmoGUI();

        m_BulletPool = new ObjectPool(bulletPrefab, maxNumOfBullets);
    }

    /*
    * 
    */
    public override void OnEnable()
    {
        base.OnEnable();

        UpdateAmmoGUI();
    }

    /*
    * Checks if reloading needs to be stopped, if so, it is stopped
    * 
    * If gun runs out of ammo and has more in pounch then reloads automatically
    */
    public override void Update()
    {
        if (TakeAction(GetReloadStartTime(), GetReloadDelay()))
            StopReloading();

        if (!HasMoreAmmo() && HasMoreAmmoInPouch())
            Reload();
    }

    /*
    * plays the shoot animation and shoots the bullet
    */
    public override void Attack()
    {
        if (bIsReloading) return; // dont do anything when gun is being reloaded
        if (!HasMoreAmmo()) return; // need to reload gun, gun doesn't have enough ammo

        if (TakeAction(m_LastAttackTime, attackDelay))
        {
            StartShooting();
        }
        else
        {
            ShootWithRecoil();
        }
    }

    /*
    * plays reload animation and reloads the gun
    */
    public override void Reload()
    {
        if (bIsReloading) return; // gun is already reloading
        if (IsAtMaxAmmo()) return; // doesn't allow reload when gun has full ammo
        if (!HasMoreAmmoInPouch()) return; // no more ammo left 

        GetAnimator().ResetTrigger(attackAnimationTriggerName);
        StopAiming();
        StartWeaponReloading();
    }

    /*
    * stops reloading
    */
    public void StopReloading()
    {
        StopWeaponReloading();
    }

    /*
    * starts aiming weapon
    */
    public override void StartAiming()
    {
        if (!bIsReloading) // if gun isn't reloading allow aim
            StartWeaponAiming();
    }

    /*
    * stops weapon aiming
    */
    public override void StopAiming()
    {
        StopWeaponAiming();
    }

    /*
     * Start Weapon Fire
     * 
     * Spawns bullet which travels in the direction of @Member Field 'fpCamera' foward vector
     * Spawn location varies on if weapon is aimed (@Member Field 'fpCamera' position vector is used), if not (@Member Field 'bulletSpawnLocation' position vector is used)
     */
    public void StartShooting()
    {
        m_LastAttackTime = Time.time;
        m_CurrentNumOfBullets--;

        GetAnimator().SetTrigger(attackAnimationTriggerName);
        Invoke("PlayMuzzleFlash", 0.1f);

        ShootBullet();
        PlayAttackAudio();
        Invoke("PlayBulletDropAudio", bulletDropAudioInterval);

        UpdateAmmoGUI();
    }

    void PlayBulletDropAudio()
    {
        SetAudioClip(bulletDropAudioClip);
        PlayAudio();
    }

    /*
    * plays the muzzle flash animation
    */
    void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    /*
     * adds recoil than shoots the bullet
     */
    public virtual void ShootWithRecoil()
    {
        Debug.Log("BaseGun 'ShoowWithRecoil()' : doesn't need an implementation, for child classes");
    }

    /*
     * Spawns bullet
     */
    private void ShootBullet()
    {
        Bullet bullet = m_BulletPool.SpawnObject() as Bullet;
        if (bIsAiming)
        {
            bullet.Spawn(fpCamera.transform.position - aimingBulletSpawnOffset, fpCamera.transform.forward, bulletRange, this);
        }
        else
        {
            bullet.Spawn(bulletSpawnLocation.position, fpCamera.transform.forward, bulletRange, this);
        }
    }

    /*
    * Start Weapon Reloading
    */
    public void StartWeaponReloading()
    {
        m_ReloadStartTime = Time.time;
        bIsReloading = true;

        GetAnimator().SetTrigger(reloadAnimationTriggerName);

        m_CurrentNumOfBullets += AmmoManager.Instance.GetAmmo(ammoType, maxNumOfBullets - m_CurrentNumOfBullets);

        Invoke("UpdateAmmoGUI", reloadDelay - reloadTimeDeductionForAmmoGUIUpdate);

        // Audio
        if (IsAudioPlaying())
            Invoke("PlayRelaodAudio", 0.2f);
        else
            PlayRelaodAudio();
    }

    void PlayRelaodAudio()
    {
        SetAudioClip(reloadAudioClip);
        PlayAudio();
    }

    /*
     * Stops Weapon Fire
     */
    public void StopWeaponReloading()
    {
        bIsReloading = false;
    }

    /*
     * Starts weapon aim
     */
    public void StartWeaponAiming()
    {
        GetAnimator().SetBool(aimDownSightAnimationBoolName, true);
        bIsAiming = true;
    }

    /*
     * Stops weapon aim
     */
    public void StopWeaponAiming()
    {
        GetAnimator().SetBool(aimDownSightAnimationBoolName, false);
        bIsAiming = false;
    }

    /*
    * returns if gun has ammo 
    */
    public bool HasMoreAmmo()
    {
        return m_CurrentNumOfBullets > 0;
    }

    /*
     * returns if num bullets are > 0
     */
    public bool HasMoreAmmoInPouch()
    {
        return AmmoManager.Instance.GetAmmoAmount(ammoType) > 0;
    }

    /*
    * returns guns ammo type
    */
    public AmmoType GetAmmoType()
    {
        return ammoType;
    }

    /*
    * returns guns current ammo
    */
    public int GetCurrentBullets()
    {
        return m_CurrentNumOfBullets;
    }

    /*
    * returns if guns ammo is at max 
    */
    public bool IsAtMaxAmmo()
    {
        return m_CurrentNumOfBullets == maxNumOfBullets;
    }

    /*
    * returns the reload start time
    */
    public float GetReloadStartTime()
    {
        return m_ReloadStartTime;
    }

    /*
    * returns reload delay time
    */
    public float GetReloadDelay()
    {
        return reloadDelay;
    }

    /*
    * returns if player can switch to another wepaon
    */
    public override bool CanSwitchWeapon()
    {
        return !bIsReloading;
    }

    /*
    * returns if the weapon is reloading or not
    */
    public override bool IsWeaponReloading()
    {
        return bIsReloading;
    }

    /*
    * returns if weapon is aimed or not
    */
    public override bool IsWeaponAimed()
    {
        return bIsAiming;
    }

    /*
    * Update the ammo gui text
    */
    public override void UpdateAmmoGUI()
    {
        if (AmmoManager.Instance != null)
            AmmoManager.Instance.UpdateAmmoGUI(ammoType, m_CurrentNumOfBullets);
    }
}
