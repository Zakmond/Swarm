using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;
    private List<GameObject> pool;

    void Awake()
    {
        pool = new List<GameObject>();
    }

    // Initialize or resize the pool with the desired size
    public void SetPoolSize(int size)
    {
        int currentSize = pool.Count;

        if (size > currentSize)
        {
            // Add new objects if the new size is larger
            for (int i = currentSize; i < size; i++)
            {
                GameObject obj = Instantiate(objectPrefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
        else if (size < currentSize)
        {
            // Disable extra objects and optionally remove them from the list
            for (int i = size; i < currentSize; i++)
            {
                Destroy(pool[i]);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        GameObject newObj = Instantiate(objectPrefab);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }
}