using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    /* Parent of all the objects in the pool, to keep the hierarchy nice and clean */
    public GameObject m_Parent;

    /* list of all objects */
    private List<PoolableObject> m_ObjectPool;

    /* current max size of the pool */
    private int m_Size = 0;

    /* count of all active objects in the pool */
    private int m_CurrentActiveObjects;

    /* prefab used to create instances for the pool */
    private PoolableObject m_ObjectPrefab;

    public ObjectPool(PoolableObject objectPrefab, int size)
    {
        m_CurrentActiveObjects = size;
        m_ObjectPool = new List<PoolableObject>(size);
        m_ObjectPrefab = objectPrefab;
        m_Parent = new GameObject(objectPrefab.name + " Pool");
        CreateObjects(size);
    }

    /*
     * Creates all of the objects
     */
    private void CreateObjects(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CreateObject();
        }
    }

    /*
     * Creates an object and adds it to the pool, also disables the object
     */
    private void CreateObject()
    {
        PoolableObject poolObject = GameObject.Instantiate(m_ObjectPrefab, Vector3.zero, Quaternion.identity, m_Parent.transform);
        poolObject.OnStart();
        poolObject.SetParent(this);
        poolObject.gameObject.SetActive(false);

        m_Size++;
    }


    /*
     * Pulls an inactive object out of pool, makes it active and returns it
     * If pool is empty, more objects are created
     */
    public PoolableObject SpawnObject()
    {
        if (m_CurrentActiveObjects >= m_Size)
        {
            //Debug.Log("Need to add more objects to pool, current size: " + m_Size);
            CreateObject();
            m_CurrentActiveObjects++;
        }

        PoolableObject poolObject = m_ObjectPool[0];
        m_ObjectPool.RemoveAt(0);
        poolObject.gameObject.SetActive(true);
        m_CurrentActiveObjects++;
        return poolObject;
    }

    /*
     * Adds the object back to pool
     */
    public void ReturnToPool(PoolableObject poolObject)
    {
        m_ObjectPool.Add(poolObject);
        m_CurrentActiveObjects--;
    }
}
