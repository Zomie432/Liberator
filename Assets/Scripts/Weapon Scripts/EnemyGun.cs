using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [SerializeField] float fireRate = 0.25f;

    [SerializeField] int damage = 25;

    /* prefab of the bullet that will be spawned in */
    [SerializeField] Bullet bulletPrefab;

    [SerializeField] Transform bulletSpawnLocation;

    /* max number of bullets this gun can have at one time */
    [SerializeField] int maxNumOfBullets = 30;

    [SerializeField] public float bulletRange = 100f;

    [SerializeField] float reloadTime = 100f;

    /* current number of bullets */
    int m_CurrentNumOfBullets;

    /* time when weapon was last fired */
    protected float m_NextTimeToFire;

    /* a objectpool that will keep track of spawned bullets automatically */
    ObjectPool m_BulletPool;

    /*
   * Creates the bullet pool 
   */
    public void Start()
    {
        m_CurrentNumOfBullets = maxNumOfBullets;

        m_BulletPool = new ObjectPool(bulletPrefab, maxNumOfBullets);
    }

    public void Shoot(Vector3 forward)
    {
        if(Time.time > m_NextTimeToFire)
        {
            Bullet bullet = m_BulletPool.SpawnObject() as Bullet;
            bullet.Spawn(bulletSpawnLocation.position, forward, bulletRange, null);
            m_NextTimeToFire = Time.time + fireRate;

            m_CurrentNumOfBullets--;
        }        

        if(m_CurrentNumOfBullets < 1)
        {
            Reload();
        }
    }

    public void Reload()
    {
        m_CurrentNumOfBullets = maxNumOfBullets;
        m_NextTimeToFire += reloadTime;
    }
}
