using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Dictionary to store multiple pools
    private Dictionary<string, List<GameObject>> poolDictionary;

    void Awake()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();
    }

    // Method to increase the pool size dynamically
    public void IncreasePoolSize(GameObject prefab, int additionalSize)
    {
        string poolKey = prefab.name;

        if (!poolDictionary.ContainsKey(poolKey))
        {
            // If the pool doesn't exist, create a new one
            poolDictionary[poolKey] = new List<GameObject>();
        }

        List<GameObject> pool = poolDictionary[poolKey];

        // Add the new objects to the pool
        for (int i = 0; i < additionalSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    // Method to decrease the pool size dynamically
    public void DecreasePoolSize(GameObject prefab, int reductionSize)
    {
        string poolKey = prefab.name;

        if (poolDictionary.ContainsKey(poolKey))
        {
            List<GameObject> pool = poolDictionary[poolKey];
            int currentSize = pool.Count;

            // Remove excess objects from the pool
            if (reductionSize > 0 && currentSize > reductionSize)
            {
                int numToRemove = Mathf.Min(reductionSize, currentSize);

                for (int i = 0; i < numToRemove; i++)
                {
                    Destroy(pool[currentSize - 1]);
                    pool.RemoveAt(currentSize - 1);
                    currentSize--;
                }
            }
        }
    }

    // Get a pooled object
    public GameObject GetPooledObject(GameObject prefab)
    {
        string poolKey = prefab.name;

        if (poolDictionary.ContainsKey(poolKey))
        {
            foreach (GameObject obj in poolDictionary[poolKey])
            {
                if (obj != null && !obj.activeInHierarchy)
                {
                    return obj;
                }
            }

            // If no inactive objects, instantiate a new one, add it to the pool, and return it
            GameObject newObj = Instantiate(prefab);
            newObj.SetActive(false);
            poolDictionary[poolKey].Add(newObj);
            return newObj;
        }

        // If the pool doesn't exist, create one with a size of 1
        IncreasePoolSize(prefab, 1);
        return poolDictionary[poolKey][0];
    }

    // Reset the pool for a specific type
    public void ResetPool(GameObject prefab)
    {
        string poolKey = prefab.name;

        if (poolDictionary.ContainsKey(poolKey))
        {
            foreach (GameObject obj in poolDictionary[poolKey])
            {
                obj.SetActive(false);
            }
        }
    }
}
