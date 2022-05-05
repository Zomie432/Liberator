using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Weapon Settings")]

    /* array of weapons */
    [SerializeField] BaseWeapon[] weapons;

    /* base throwable of type flashbang */
    [SerializeField] BaseThrowables flashbang;

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

    [SerializeField] float animationSpeedInterpTime = 0.1f;

    /* current equipped weapon */
    int m_CurrentWeaponIndex;

    /* current player health */
    int m_CurrentPlayerHealth;

    /* current player shield */
    int m_CurrentPlayerShield;

    /* Last Animation speed */
    float m_LastAnimSpeed = 0f;

    /* player motor of the player */
    PlayerMotor m_PlayerMotor;

    /* current equipped weapon */
    BaseWeapon m_CurrentEquippedWeapon;

    private void Awake()
    {
        m_CurrentWeaponIndex = 0;

        m_CurrentPlayerHealth = maxPlayerHealth;
        m_CurrentPlayerShield = maxPlayerShield;

        m_CurrentEquippedWeapon = weapons[m_CurrentWeaponIndex];

        m_PlayerMotor = GetComponent<PlayerMotor>();
    }

    private void Start()
    {
        m_CurrentEquippedWeapon.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;

        //weapons[1].Start();
        //flashbang.Start();

        //DeactivateWeapon(1);
        //DeactivateFlashbang();
    }

    private void Update()
    {
            SetCurrentAnimSpeed(m_PlayerMotor.currentActiveSpeed2D);

            if (m_CurrentEquippedWeapon.IsWeaponAimed())
                m_PlayerMotor.SlowWalk();

            if(flashbang.isActiveAndEnabled && !flashbang.HasThrowablesLeft())
                 EquipPreviousWeapon();
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

    public void EquipFlashbang()
    {
        if (flashbang.HasThrowablesLeft())
        {
            ActivateFlashbang();
            DeactivateWeapon(m_CurrentWeaponIndex);
        }
    }

    /*
     * equips weapon one
     */
    public void EquipWeaponOnePressed()
    {
        EquipWeapon(0);
    }

    /*
     * equips weapon two
     */
    public void EquipWeaponTwoPressed()
    {
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

        ActivateWeapon(index);
        DeactivateWeapon(m_CurrentWeaponIndex);
        
        m_CurrentWeaponIndex = index;
    }

    private void ActivateWeapon(int index)
    {
        weapons[index].SetActive(true);
        m_CurrentEquippedWeapon = weapons[index];
    }

    private void ActivateFlashbang()
    {
        flashbang.SetActive(true);
        m_CurrentEquippedWeapon = flashbang;
    }

    private void DeactivateWeapon(int index)
    {
        weapons[index].SetActive(false);
    }

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
    int GetPlayersMaxHealth()
    {
        return maxPlayerHealth;
    }

    /*
    * returns players current health
    */
    int GetCurrentPlayerHealth()
    {
        return m_CurrentPlayerHealth;
    }

    /*
    * returns players max shield
    */
    int GetPlayersMaxShield()
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
    int GetCurrentPlayerShield()
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
}
