using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    /* objects lifetime before it gets pooled, negative means infinite */
    [SerializeField] [Tooltip("-1 = don't auto pool, positive: pool in seconds")] protected float autoPoolTime = 5f;

    /* The parent object pool this object belongs to */
    private ObjectPool m_Parent;

    /* Safe keep boolean check to eliminate multiple pools of this object */
    protected bool m_IsAlreadyPooled = false;

    /*
     * called once when the object is created
     */
    public virtual void OnStart()
    {

    }

    public virtual void OnEnable()
    {
        m_IsAlreadyPooled = false;

        if (autoPoolTime > 0)
            Invoke("Pool", autoPoolTime);
    }

    /*
     * When object is disabled, it returns to the parents pool
     */
    public virtual void OnDisable()
    {
        m_Parent.ReturnToPool(this);
    }

    /*
     * Calls the OnDisable() function, which returns object to their object pools
     */
    protected void Pool()
    {
        if (!m_IsAlreadyPooled)
        {
            gameObject.SetActive(false);
            m_IsAlreadyPooled = true;
        }
    }
    public void SetParent(ObjectPool parent)
    {
        m_Parent = parent;
    }
}
