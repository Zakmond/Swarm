using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Dictionary<string, List<GameObject>> poolDictionary;

    void Awake()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();
    }

    public void IncreasePoolSize(GameObject prefab, int additionalSize)
    {
        string poolKey = prefab.name;

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary[poolKey] = new List<GameObject>();
        }

        List<GameObject> pool = poolDictionary[poolKey];

        for (int i = 0; i < additionalSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public void DecreasePoolSize(GameObject prefab, int reductionSize)
    {
        string poolKey = prefab.name;

        if (poolDictionary.ContainsKey(poolKey))
        {
            List<GameObject> pool = poolDictionary[poolKey];
            int currentSize = pool.Count;

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

            GameObject newObj = Instantiate(prefab);
            newObj.SetActive(false);
            poolDictionary[poolKey].Add(newObj);
            return newObj;
        }

        IncreasePoolSize(prefab, 1);
        return poolDictionary[poolKey][0];
    }

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
