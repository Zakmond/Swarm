using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLevelManager : MonoBehaviour
{
    [System.Serializable]
    public class MonsterData
    {
        public string type; // Prefab name
        public int count; // Number of monsters
        public float health; // Health modifier
        public float speed; // Speed modifier
        public float damage; // Damage modifier
        public float attackDistance; // Attack distance modifier
    }

    [System.Serializable]
    public class MonsterWave
    {
        public float spawnTime; // Time to spawn the wave
        public MonsterData[] monsters; // Array of monsters in the wave
    }

    [System.Serializable]
    public class LevelConfig
    {
        public int levelNumber;
        public float levelDuration; // Level time limit
        public MonsterWave[] monsterWaves; // Array of waves
    }

    public TextAsset levelConfigFile; // JSON file with NPC data
    public Transform spawnPoint; // Spawn point for monsters
    public ObjectPool objectPool; // Reference to the object pool
    public LevelConfig levelConfig;

    private void Start()
    {
        // Load level configuration
        LoadLevelConfig();

        // Preload the monsters into the pool
        PreloadMonsters();

        // Start spawning NPCs
        StartCoroutine(SpawnNPCs());
    }

    private void LoadLevelConfig()
    {
        if (levelConfigFile != null)
        {
            levelConfig = JsonUtility.FromJson<LevelConfig>(levelConfigFile.text);
            Debug.Log("Level configuration loaded.");
        }
        else
        {
            Debug.LogError("No level configuration file assigned!");
        }
    }

    private void PreloadMonsters()
    {
        // Loop through all waves and preload the required number of monsters
        foreach (var wave in levelConfig.monsterWaves)
        {
            foreach (var monster in wave.monsters)
            {
                GameObject prefab = Resources.Load<GameObject>(monster.type);
                if (prefab == null)
                {
                    Debug.LogError($"Prefab for {monster.type} not found in Resources!");
                    continue;
                }

                // Calculate the total number of monsters needed for this type
                objectPool.IncreasePoolSize(prefab, monster.count);
            }
        }

        Debug.Log("All monsters preloaded into the pool.");
    }

    private IEnumerator SpawnNPCs()
    {
        foreach (var wave in levelConfig.monsterWaves)
        {
            yield return new WaitForSeconds(wave.spawnTime);

            foreach (var monster in wave.monsters)
            {
                GameObject prefab = Resources.Load<GameObject>(monster.type);
                if (prefab == null)
                {
                    Debug.LogError($"Prefab for {monster.type} not found in Resources!");
                    continue;
                }

                for (int i = 0; i < monster.count; i++)
                {
                    GameObject instance = objectPool.GetPooledObject(prefab);
                    if (instance != null)
                    {
                        // Update the monster's stats based on the wave
                        NPCBehavior behavior = instance.GetComponent<NPCBehavior>();
                        if (behavior != null)
                        {
                            behavior.UpdateNPC(monster.health, monster.speed, monster.attackDistance, monster.damage);
                        }

                        // Activate and position the monster
                        instance.transform.position = spawnPoint.position;
                        instance.SetActive(true);
                    }

                    yield return new WaitForSeconds(0.5f); // Delay between spawns
                }
            }
        }
    }
}
