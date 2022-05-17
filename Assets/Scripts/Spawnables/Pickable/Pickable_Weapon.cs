using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable_Weapon : IPickable
{
    [SerializeField] uint weaponID;

    public override void OnDrop()
    {
        base.OnDrop();

        gameObject.SetActive(true);
    }

    public override void Despawn()
    {
        base.Despawn();

        gameObject.SetActive(false);
    }

    public override void OnPickup(GameObject picker)
    {
        //base.OnPickup(picker); don't want it to despawn

        if (GameManager.Instance.playerScript.CanPickupWeapon())
        {
            Despawn();
            GameManager.Instance.playerScript.Equip(weaponID, transform);
        }
    }
}
