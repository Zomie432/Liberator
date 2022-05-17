using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager
{
    private static Dictionary<string, ObjectPool> m_ObjectPools =  new Dictionary<string, ObjectPool>();

    public static string CreateObjectPool(PoolableObject objectPrefab, int size)
    {
        if (!m_ObjectPools.ContainsKey(objectPrefab.name + " Pool"))
        {
            // Create a new object pool
            ObjectPool pool = new ObjectPool(objectPrefab, size);
            m_ObjectPools.Add(objectPrefab.name + " Pool", pool);
        }

        return objectPrefab.name + " Pool";
    }

    public static PoolableObject SpawnObject(string poolName)
    {
       return m_ObjectPools[poolName].SpawnObject();
    }
}
