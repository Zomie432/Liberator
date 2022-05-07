using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    /* The type of ammo being picked up */
    [SerializeField] AmmoType ammoType;

    /* Amount of ammo given upon pickup */
    [SerializeField] int ammoAmount;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            AmmoManager.Instance.IncreaseAmmo(ammoType, ammoAmount);
            Destroy(gameObject);
        }
    }
}
