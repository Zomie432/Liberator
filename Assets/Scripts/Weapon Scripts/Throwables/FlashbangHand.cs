using UnityEngine;

public class FlashbangHand : BaseWeapon
{
    /*
    * 
    */
    [SerializeField] float throwForceMultiplier = 5.0f;

    [SerializeField] float distanceFromPlayerMultiplier = 5.0f;

    [SerializeField] Flashbang flashbangPrefab;

    [SerializeField] int maxFlashbangAmount = 2;

    [SerializeField] Transform flashbangSpawnLocation;

    int m_CurrentFlashbangAmount;
    private ObjectPool m_FlashbangPool;

    private bool bIsThrowing = false;

    public FlashbangHand()
    {
        m_CurrentFlashbangAmount = 1;
    }

    public override void Start()
    {
        base.Start();

        m_FlashbangPool = new ObjectPool(flashbangPrefab, maxFlashbangAmount);
        m_CurrentFlashbangAmount = maxFlashbangAmount;

        GameManager.Instance.playerScript.UpdateFlashbangCount();

        Debug.Log("Start");
    }

    public override void OnEnable()
    {
        base.OnEnable();

        AmmoManager.Instance.HideAmmoGUI();
        GameManager.Instance.playerScript.ShowFlashbangGUI();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        AmmoManager.Instance.ShowAmmoGUI();
        GameManager.Instance.playerScript.HideFlashbangGUI();
    }

    public override void Attack()
    {
        if (!HasMoreFlashbangs()) return;

        if (TakeAction(m_LastAttackTime, attackDelay))
        {
            bIsThrowing = true;

            PlayAttackAudio();

            GetAnimator().SetTrigger(attackAnimationTriggerName);
            UpdateLastAttackTime();

            m_CurrentFlashbangAmount--;
        }
    }

    public void OnThrowEvent()
    {
        Flashbang flashbang = m_FlashbangPool.SpawnObject() as Flashbang;
        flashbang.transform.position = flashbangSpawnLocation.position;
        flashbang.transform.forward = flashbangSpawnLocation.forward;

        Vector3 targetPoint = fpCamera.transform.position + fpCamera.transform.forward * distanceFromPlayerMultiplier;
        Vector3 direction = (targetPoint - flashbangSpawnLocation.position).normalized;

        Debug.DrawLine(flashbangSpawnLocation.position, targetPoint, Color.red, 2f);

        flashbang.OnThrowThrowable(direction, throwForceMultiplier);
    }

    public void OnThrowAnimEnded()
    {
        bIsThrowing = false;
    }

    public override bool CanSwitchWeapon()
    {
        return !bIsThrowing;
    }

    public bool HasMoreFlashbangs()
    {
        return m_CurrentFlashbangAmount > 0;
    }

    public int GetMaxAmountOfFlashbangs()
    {
        return maxFlashbangAmount;
    }

    public int GetCurrentAmountOfFlashbangs()
    {
        return m_CurrentFlashbangAmount;
    }
}
