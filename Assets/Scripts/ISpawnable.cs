
using UnityEngine;

public class ISpawnable : MonoBehaviour
{
    Vector3 m_InitialPosition;
    Quaternion m_InitialRotation;

    public virtual void Spawn()
    {
        //Debug.Log(name + ", has been spawned!");

        m_InitialPosition = transform.position;
        m_InitialRotation = transform.rotation;
    }

    public virtual void Despawn() 
    { 
        //Debug.Log(name + ", has been de-spawned!"); 
    }

    public virtual void Respawn()
    {
        //Debug.Log(name + ", has been re-spawned!");9*

        transform.position = m_InitialPosition;
        transform.rotation = m_InitialRotation;
    }
}
