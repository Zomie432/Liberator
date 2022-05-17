using UnityEngine;

public class FlashbangHand : BaseWeapon
{
    /* scales the force being applied to the flash bang */
    [SerializeField] [Tooltip("scales the force being applied to the flash bang")] float throwForceMultiplier = 5.0f;

    /* the amount of distance the flashbang should go to before exploding */
    [SerializeField] [Tooltip("the amount of distance the flashbang should go to before exploding")] float distanceFromPlayerMultiplier = 5.0f;

    /* The flash bang prefab used to create the object pool of flashbangs */
    [SerializeField] [Tooltip("The flash bang prefab used to create the object pool of flashbangs")] Flashbang flashbangPrefab;

    /* max amount of flashbangs the player can hold at a given time */
    [SerializeField] [Tooltip("max amount of flashbangs the player can hold at a given time")] int maxFlashbangAmount = 2;

    /* the spawn location of the flashbang, gets sets before it gets thrown */
    [SerializeField] [Tooltip("the spawn location of the flashbang, gets sets before it gets thrown")] Transform flashbangSpawnLocation;

    /* current amount of flashbangs */
    int m_CurrentFlashbangAmount;

    /* object pool of flashbangs */
    private string m_FlashbangPool;

    public FlashbangHand()
    {
        m_CurrentFlashbangAmount = 1;
    }

    public override void Start()
    {
        base.Start();

        m_FlashbangPool = ObjectPoolManager.CreateObjectPool(flashbangPrefab, maxFlashbangAmount);
        m_CurrentFlashbangAmount = maxFlashbangAmount;

        GameManager.Instance.playerScript.UpdateFlashbangCount();
    }

    /*
    * Hides the ammo GUI
    * Shows the flashbangCount GUI
    */
    public override void OnWeaponEquip()
    {
        AmmoManager.Instance.HideAmmoGUI();
        GameManager.Instance.playerScript.ShowFlashbangGUI();
    }

    /*
    * Shows the ammo GUI
    * Hides the flashbangCount GUI
    */
    public override void OnWeaponUnequip()
    {
        AmmoManager.Instance.ShowAmmoGUI();
        GameManager.Instance.playerScript.HideFlashbangGUI();
    }

    /*
    * Called when the player wants to throw the flashbang
    *   - if player has mroe flashbangs and the attack delay is met then it allows to throw the flashbang
    *   - turn bIsThrowing on
    *   - the it plays the attack audio
    *   - Sets the animator to play the throwing animation and updates the last attack time
    *   - Decrements the current amount of flashbangs left
    */
    public override void StartAttacking()
    {
        if (!HasMoreFlashbangs()) return;

        if (TakeAction(m_LastAttackTime, attackRate))
        {
            PlayAttackAudio();

            GetAnimator().Play("Attack1");
            UpdateLastAttackTime();

            m_CurrentFlashbangAmount--;
        }
    }

    /*
    * Called when the throwing animation is at where the flashbang should be throw'd
    *   - spawns a flashbang from the flashbang pool
    *   - finds the targetpoint the flashbang needs to reach
    *   - calls the flashbangs OnThrowThowable() method to let the flashbang know player is throwing  it
    */
    public void OnAnimationEvent_Throw()
    {
        Flashbang flashbang = ObjectPoolManager.SpawnObject(m_FlashbangPool) as Flashbang;
        flashbang.transform.position = flashbangSpawnLocation.position;
        flashbang.transform.forward = flashbangSpawnLocation.forward;

        Vector3 targetPoint = GameManager.Instance.mainCamera.transform.position + GameManager.Instance.mainCamera.transform.forward * distanceFromPlayerMultiplier;
        Vector3 direction = (targetPoint - flashbangSpawnLocation.position).normalized;

        Debug.DrawLine(flashbangSpawnLocation.position, targetPoint, Color.red, 2f);

        flashbang.OnThrowThrowable(direction, throwForceMultiplier);
    }

    /*
    * returns if player can switch from the flashbang to another weapon
    */
    public override bool CanSwitchWeapon()
    {
        return !bIsAttacking;
    }

    /*
    * returns if theirs any flashbangs left
    */
    public bool HasMoreFlashbangs()
    {
        return m_CurrentFlashbangAmount > 0;
    }

    /*
    * returns the max amount of flashbangs 
    */
    public int GetMaxAmountOfFlashbangs()
    {
        return maxFlashbangAmount;
    }

    /*
    * returns the current amount of flashbangs left 
    */
    public int GetCurrentAmountOfFlashbangs()
    {
        return m_CurrentFlashbangAmount;
    }
}
