using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Health health;

    public void OnRaycastHit(BaseWeapon weapon, Vector3 direction)
    {
        //modify 25f with the bullet.damage after implementation.
        if (IsEnemyDead())
        {
            return;
        }

        health.TakeDamage(weapon.GetDamage(), direction);
    }

    public bool IsEnemyDead()
    {
        return health.currentHealth < 1f;
    }

}
