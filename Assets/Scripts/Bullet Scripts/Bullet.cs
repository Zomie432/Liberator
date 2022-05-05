using UnityEngine;

public class Bullet : PoolableObject
{
    /* time delay before bullet is pooled after collision */
    public float destroyTimeAfterCollision = 1f;

    /* speed of bullet */
    public float moveSpeed = 25f;

    /* name of the method that is invoked after collision to pool object */
    private const string DISABLE_METHOD_NAME = "ReturnBulletToPool";

    /* bullet collided bool */
    bool bHitSomething = false;

    /* spawned position of bullet */
    Vector3 m_StartPosition;

    /* current position of bullet */
    Vector3 m_CurrentPosition;

    /* next position of bullet */
    Vector3 m_NextPosition;

    /* range the bullet can go to before it gets pooled */
    float m_BulletRange = 0f;

    BaseGun parentGun;

    /*
     * Overriden to set @Member Field 'bHitSomething' when bullet is active again
     */
    public override void OnEnable()
    {
        m_IsAlreadyPooled = false;

        if (autoPoolTime > 0)
            Invoke("Pool", autoPoolTime);

        bHitSomething = false;
    }

    /*
     * Raycasts casted to check if bullet collided with an object, if so, OnRayCastHit, is called
     * If bullet exceeds teh bullet range, it gets pooled
     */
    private void FixedUpdate()
    {
        float nextPositionMultiplier = moveSpeed * Time.deltaTime;
        m_CurrentPosition = transform.position;
        m_NextPosition = transform.position + (transform.forward * nextPositionMultiplier);

        if (!bHitSomething)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(m_CurrentPosition, transform.forward, out hitInfo, nextPositionMultiplier))
            {
                bHitSomething = true;
                OnRayCastHit(hitInfo);
            }
        }

        transform.position = m_NextPosition;

        if ((m_StartPosition - transform.position).magnitude >= m_BulletRange)
            ReturnBulletToPool();
    }

    /*
     * Bullet collided with an object
     * Spawns a impact particle based on what was hit
     * Invokes disable method for bullet to get pooled
     */
    void OnRayCastHit(RaycastHit hit)
    {
        if(hit.collider.tag == "Hitbox")
        {
           hit.collider.GetComponent<Hitbox>().OnRaycastHit(parentGun, transform.forward);
        }
        

        BulletImpactManager.Instance.SpawnBulletImpact(hit.point, hit.normal, hit.collider.tag);

        CancelInvoke(DISABLE_METHOD_NAME);
        Invoke(DISABLE_METHOD_NAME, destroyTimeAfterCollision);
    }

    /*
     * Updates member fields when bullet is spawned
     */
    public void Spawn(Vector3 position, Vector3 forward, float bulletRange, BaseGun parent)
    {
        m_StartPosition = position;
        transform.position = position;
        transform.forward = forward;

        parentGun = parent;
    
        m_BulletRange = bulletRange;
    }

    #region CollisionDetection
    /*private void OnCollisionEnter(Collision collision) // Not beings used for this projectile as it moves faster than the collision checks in one frame
    {
        ContactPoint contact = collision.GetContact(0);

        if (collision.gameObject.TryGetComponent<Renderer>(out Renderer renderer))
        {
            BulletImpactManager.Instance.SpawnBulletImpact(contact.point, contact.normal, renderer.sharedMaterial);
        }
        else
        {
            BulletImpactManager.Instance.SpawnBulletImpact(contact.point, contact.normal, null);
        }

        CancelInvoke(DISABLE_METHOD_NAME);
        Invoke(DISABLE_METHOD_NAME, DestroyTimeAfterCollision);
    }*/
    #endregion // Not in use

    /*
     * Returns bullet to it pool
     */
    private void ReturnBulletToPool()
    {
        Pool();
    }
}