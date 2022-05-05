using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Health health;

    public void OnRaycastHit(BaseWeapon weapon, Vector3 direction)
    {
        //modify 25f with the bullet.damage after implementation.
        health.TakeDamage(weapon.GetDamage(), direction);
    }

}
