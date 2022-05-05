using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    /* The type of ammo being picked up */
    [SerializeField] AmmoType ammoType;

    /* Amount of ammo given upon pickup */
    [SerializeField] int ammoAmount;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            AmmoManager.Instance.IncreaseAmmo(ammoType, ammoAmount);
            Destroy(gameObject);
        } 
    }
}
