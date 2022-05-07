using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Weapon Settings")]

    /* array of weapons */
    [SerializeField] BaseWeapon[] weapons;

    /* base throwable of type flashbang */
    [SerializeField] FlashBang flashbang;

    [Header("Player Settings")]

    ///* keycode used to shoot weapon */
    //[SerializeField] KeyCode shootKey = KeyCode.Mouse0;

    ///* keycode used to aim weapon */
    //[SerializeField] KeyCode aimKey = KeyCode.Mouse1;

    ///* keycode used to reload weapon */
    //[SerializeField] KeyCode reloadKey = KeyCode.R;

    ///* keycode used to switch to next weapon */
    //[SerializeField] KeyCode switchToNextWeaponKey = KeyCode.Alpha1;

    ///* keycode used to switch to previous weapon */
    //[SerializeField] KeyCode switchToPreviousWeaponKey = KeyCode.Alpha2;

    /* max health player can have */
    [SerializeField] int maxPlayerHealth = 100;

    /* max health player can have */
    [SerializeField] int maxPlayerShield = 100;

    /* amount of percentage of damage the shield can intake when player has been damaged */
    [SerializeField] float playerShieldFallOffScale = 0.8f;

    [Header("Animation Settings")]

    /* time it takes to lerp from last animation speed to current animation speed */
    [SerializeField] float animationSpeedInterpTime = 0.1f;

    [Header("Audio Settings")]

    /* footstep audio when player walks */
    [SerializeField] AudioClip footStepWalkAudio;

    /* footstep audio when player runs */
    [SerializeField] AudioClip footStepRunAudio;

    /* delay on the foot step walk audio to keep it in control */
    [SerializeField] float footStepWalkAudioPlayDelay = 0.75f;

    /* delay on the foot step run audio to keep it in control */
    [SerializeField] float footStepRunAudioPlayDelay = 0.75f;

    /* current equipped weapon */
    int m_CurrentWeaponIndex;

    /* current player health */
    int m_CurrentPlayerHealth;

    /* current player shield */
    int m_CurrentPlayerShield;

    /* Last Animation speed */
    float m_LastAnimSpeed = 0f;

    /* audio source used for footsteps */
    AudioSource m_FootstepAudioSrc;

    /* last time a sound effect was played for footsteps */
    float m_LastStepSoundTime = 0f;

    /* player motor of the player */
    PlayerMotor m_PlayerMotor;

    /* current equipped weapon */
    BaseWeapon m_CurrentEquippedWeapon;

    /* Health bar connected to player, in inspector must select health bar from ui to work*/
    public HealthBar healthBar;

    /* Shield bar connected to player, in inspector must select Shield bar from ui to work*/
    public ShieldBar shieldBar;
    private void Start()
    {
        m_CurrentWeaponIndex = 0;

        m_CurrentPlayerHealth = maxPlayerHealth;
        m_CurrentPlayerShield = maxPlayerShield;
        healthBar.SetMaxHealth();
        shieldBar.SetMaxShield();
        m_CurrentEquippedWeapon = weapons[m_CurrentWeaponIndex];

        m_PlayerMotor = GetComponent<PlayerMotor>();
        m_FootstepAudioSrc = GetComponentInChildren<AudioSource>();

        m_CurrentEquippedWeapon.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (m_PlayerMotor.IsPlayerStrafing() || m_PlayerMotor.IsPlayerWalkingBackwards())
        {
            SetCurrentAnimSpeed(0.3f);
        }
        else
        {
            SetCurrentAnimSpeed(m_PlayerMotor.currentActiveSpeed2D);
        }

        if(m_PlayerMotor.currentActiveSpeed2D > 0.1f)
        {
            if (Time.time - m_LastStepSoundTime > footStepWalkAudioPlayDelay && m_PlayerMotor.currentActiveSpeed2D < 0.3f)
            {
                SetFootstepAudio(footStepWalkAudio);
                PlayFootStepAudio();
                m_LastStepSoundTime = Time.time;
            }
            else if(Time.time - m_LastStepSoundTime > footStepRunAudioPlayDelay && m_PlayerMotor.currentActiveSpeed2D > 0.3f)
            {
                SetFootstepAudio(footStepRunAudio);
                PlayFootStepAudio();
                m_LastStepSoundTime = Time.time;
            }
        }

        

        if (m_CurrentEquippedWeapon.IsWeaponAimed())
            m_PlayerMotor.SlowWalk();

        if (flashbang.isActiveAndEnabled && !flashbang.HasThrowablesLeft())
            EquipPreviousWeapon();

        if(flashbang.ShouldPlayerBeFlashed())
        {
            flashbang.Update();
        }
    }

    /*
     * Equips a weapon, drops current weapon and equips the weapon picked up
     */
    void Equip(BaseWeapon weapon)
    {

    }

    /*
     * Unequips current weapon
     */
    void UnEquip(BaseWeapon weapon)
    {

    }

    /*
     * DEBUG_-----------------------------------------------------------------------------
     */
    public void TakeDamageTen()
    {
        TakeDamage(10);
    }

    public void EquipFlashbang()
    {
        if (flashbang.HasThrowablesLeft())
        {
            DeactivateWeapon(m_CurrentWeaponIndex);
            ActivateFlashbang();
        }
    }

    /*
     * equips weapon one
     */
    public void EquipWeaponOnePressed()
    {
        if(flashbang.isActiveAndEnabled)
            ForceEquipWeapon(0);
        else
            EquipWeapon(0);
    }

    /*
     * equips weapon two
     */
    public void EquipWeaponTwoPressed()
    {
        if (flashbang.isActiveAndEnabled)
            ForceEquipWeapon(1);
        else
            EquipWeapon(1);
    }

    /*
     * called when player presses the button to equip next weapon
     */
    public void OnEquipNextPressed()
    {
        EquipNextWeapon();
    }

    /*
     * Equips next weapon
     */
    void EquipNextWeapon()
    {
        EquipWeapon(m_CurrentWeaponIndex + 1);
    }

    /*
     * called when player presses the button to equip next weapon
     */
    public void OnEquipPreviousPressed()
    {
        EquipPreviousWeapon();
    }

    /*
     * Equips previous weapon
     */
    void EquipPreviousWeapon()
    {
        EquipWeapon(m_CurrentWeaponIndex - 1);
    }

    /*
     * equips a weapon based on the index passed in
     */
    void EquipWeapon(int index)
    {
        if (index == m_CurrentWeaponIndex || !m_CurrentEquippedWeapon.CanSwitchWeapon()) return;

        if (flashbang.isActiveAndEnabled)
            DeactivateFlashbang();

        if (index == weapons.Length)
            index = 0;

        if (index < 0)
            index = weapons.Length - 1;

        DeactivateWeapon(m_CurrentWeaponIndex); // order matters, since we needs this to disable first so its OnDisable() method could be called first
        ActivateWeapon(index);

        m_CurrentWeaponIndex = index;
    }

    /*
     * equips a weapon based on the index passed in, but its assumed to be a correct index, forces player to switch to the index
     */
    void ForceEquipWeapon(int index)
    {
        if (!m_CurrentEquippedWeapon.CanSwitchWeapon()) return;

        if (flashbang.isActiveAndEnabled)
            DeactivateFlashbang();

        DeactivateWeapon(m_CurrentWeaponIndex);
        ActivateWeapon(index);

        m_CurrentWeaponIndex = index;
    }

    /*
    * enables the weapon at @Param: 'index', and sets it to current equipped weapon
    */
    private void ActivateWeapon(int index)
    {
        weapons[index].SetActive(true);
        m_CurrentEquippedWeapon = weapons[index];
    }

    /*
    * enables flashbang, and sets it to current equipped weapon
    */
    private void ActivateFlashbang()
    {
        flashbang.SetActive(true);
        m_CurrentEquippedWeapon = flashbang;
    }

    /*
    * disables the weapon at @Param: 'index'
    */
    private void DeactivateWeapon(int index)
    {
        m_CurrentEquippedWeapon.OnWeaponSwitch();
        weapons[index].SetActive(false);
    }

    /*
    * disables flashbang
    */
    private void DeactivateFlashbang()
    {
        flashbang.SetActive(false);
    }

    /*
     * Damages player
     * 
     * If shield is not empty, it finds the amount of damage the shield will take, which is returned by the GetShieldDamage() method,
     * then it decreases the players shield, finally decrease players health
     */
    public void TakeDamage(int damage)
    {
        int shieldDamageFallOff = 0;
        if (!IsPlayerShieldEmpty())
            shieldDamageFallOff = GetShieldDamage(damage);

        shieldDamageFallOff = DecreasePlayerShield(shieldDamageFallOff);
        DecreasePlayerHealth(damage - shieldDamageFallOff);
        healthBar.UpdateHealthBar();
        shieldBar.UpdateShieldBar();
        Debug.Log("Damage Taken: " + damage);
        Debug.Log("Current Health: " + m_CurrentPlayerHealth);
        Debug.Log("Current Shield: " + m_CurrentPlayerShield);
    }

    /*
     * Decreases players health, if health goes <= zero then it calls the PlayerDied() method
     */
    void DecreasePlayerHealth(int amount)
    {
        m_CurrentPlayerHealth -= amount;

        if (m_CurrentPlayerHealth <= 0) // Player died
        {
            PlayerDied();
        }
    }

    /*
     * Increases players health, if health goes > max ealth then just sets current health to max health
     */
    void IncreasePlayerHealth(int amount)
    {
        m_CurrentPlayerHealth += amount;

        if (amount > GetPlayersMaxHealth())
            m_CurrentPlayerHealth = GetPlayersMaxHealth();
    }

    /*
     * Decreases players shields
     * 
     * First finds out if shield can withstand the incoming amount of damage, if not, take the amount of damage the shield can handle and return that amount
     */
    int DecreasePlayerShield(int amount)
    {
        int shieldAmount = m_CurrentPlayerShield - amount;

        if (shieldAmount < 0)
            amount = m_CurrentPlayerShield;

        m_CurrentPlayerShield -= amount;

        return amount;
    }

    /*
    * Increases players shield, if shield goes > max shield then just sets current shield to max shield
    */
    void IncreasePlayerShield(int amount)
    {
        m_CurrentPlayerShield += amount;

        if (amount > GetPlayersMaxShield())
            m_CurrentPlayerShield = GetPlayersMaxShield();
    }

    /*
     * Called when players dies 
     */
    void PlayerDied()
    {
        Debug.Log("Player hassss died!!!");
    }

    /*
     * called when user presses attack button
     */
    public void OnAttackPressed()
    {
        StartAttacking();
    }

    /*
     * called when users holds down the attack button
     */
    public void OnAttackHold()
    {
        if (m_CurrentEquippedWeapon.CanPlayerHoldAttackTrigger())
            StartAttacking();
    }

    /*
     * called when user presses ads button
     */
    public void OnADSPressed()
    {
        if (!m_CurrentEquippedWeapon.CanPlayerHoldAimTrigger() && m_CurrentEquippedWeapon.IsWeaponAimed())
            StopAiming();
        else
            StartAiming();
    }

    /*
    * called when user releases ads button
    */
    public void OnADSReleased()
    {
        if (m_CurrentEquippedWeapon.CanPlayerHoldAimTrigger())
            StopAiming();
    }

    /*
     * called when user presses reload button
     */
    public void OnReloadPressed()
    {
        StartReloading();
    }

    /*
     * starts weapon shooting
     */
    void StartAttacking()
    {
        m_CurrentEquippedWeapon.Attack();
    }

    /*
     * starts weapon reloading
     */
    void StartReloading()
    {
        m_CurrentEquippedWeapon.Reload();
    }

    /*
     * starts weapon aiming
     */
    void StartAiming()
    {
        m_CurrentEquippedWeapon.StartAiming();
    }

    /*
     * stops weapon aiming
     */
    void StopAiming()
    {
        m_CurrentEquippedWeapon.StopAiming();
    }

    /*
    * lerps from last aniamtion speed to current animation speed to create a flow
    */
    public void SetCurrentAnimSpeed(float speed)
    {
        m_LastAnimSpeed = Mathf.Lerp(m_LastAnimSpeed, speed, animationSpeedInterpTime);
        m_CurrentEquippedWeapon.SetAnimFloat("Speed", m_LastAnimSpeed);
    }

    /*
    * Update the ammo gui text
    */
    public void UpdateAmmoGUI()
    {
        m_CurrentEquippedWeapon.UpdateAmmoGUI();
    }

    /*
    * returns players max health
    */
    public int GetPlayersMaxHealth()
    {
        return maxPlayerHealth;
    }

    /*
    * returns players current health
    */
    public int GetCurrentPlayerHealth()
    {
        return m_CurrentPlayerHealth;
    }

    /*
    * returns players max shield
    */
    public int GetPlayersMaxShield()
    {
        return maxPlayerShield;
    }

    /*
    * returns if players shield has ran out
    */
    bool IsPlayerShieldEmpty()
    {
        return m_CurrentPlayerShield <= 0;
    }

    /*
    * returns players current shield amount
    */
    public int GetCurrentPlayerShield()
    {
        return m_CurrentPlayerShield;
    }

    /*
    * returns the amount of shield needed to be taken off upon some damage
    */
    int GetShieldDamage(int damageTaken)
    {
        return (int)(damageTaken * playerShieldFallOffScale);
    }

    /*
    * set the footstep audio sources clip to the @Param: 'clip'
    */
    void SetFootstepAudio(AudioClip clip)
    {
        m_FootstepAudioSrc.clip = clip;
    }

    /*
    * plays a footstep audio
    */
    void PlayFootStepAudio()
    {
        m_FootstepAudioSrc.Play();
    }
}
