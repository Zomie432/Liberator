public interface IWeapon
{
    /*
     * triggers weapon attack
     */
    void Attack();

    /*
     * triggers weapon reload
     */
    void Reload();

    /*
     * starts weapon aim
     */
    void StartAiming();

    /*
     * stops weapon aim
     */
    void StopAiming();

    /*
     * enables/disables gameObject
     */
    void SetActive(bool isActive);
}
