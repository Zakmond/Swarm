using System;
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
        public float healthModifier; // Health modifier
        public float speedModifier; // Speed modifier
        public float damageModifier; // Damage modifier
        public float attackDistanceModifier; // Attack distance modifier
        public String[] spawnPoints;
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
    public ObjectPool objectPool; // Reference to the object pool
    public LevelConfig levelConfig;
    public Dictionary<string, Transform> spawnPoints = new();
    public int mobCount = 0;
    public static NPCLevelManager Instance;
    public int mobsKilled = 0;
    public event Action<bool> OnWin;
    public event Action<bool> OnLoss;
    public float levelTimer;

    private void Awake()
    {
        // Singleton pattern to persist across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep alive across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (Transform child in GameObject.Find("SpawnPoints").transform)
        {
            spawnPoints[child.name] = child;
        }
    }
    private void Start()
    {
        // Load level configuration
        LoadLevelConfig();

        // Preload the monsters into the pool
        PreloadMonsters();

        // Start spawning NPCs
        StartCoroutine(SpawnNPCs());
    }
    private Vector2 GetRandomPositionNearPoint(Transform spawnPoint, float offset)
    {
        return new Vector2(
            spawnPoint.position.x + UnityEngine.Random.Range(-offset, offset),
            spawnPoint.position.y + UnityEngine.Random.Range(-offset, offset)
        );
    }
    private void LoadLevelConfig()
    {
        if (levelConfigFile != null)
        {
            levelConfig = JsonUtility.FromJson<LevelConfig>(levelConfigFile.text);
            levelTimer = levelConfig.levelDuration;
            Debug.Log("Level configuration loaded.");
        }
        else
        {
            Debug.LogError("No level configuration file assigned!");
        }
    }

    private void Update()
    {

        levelTimer -= Time.deltaTime;

        if (levelTimer <= 0)
        {
            OnLoss?.Invoke(true);

        }

    }

    private void PreloadMonsters()
    {
        // Loop through all waves and preload the required number of monsters
        foreach (var wave in levelConfig.monsterWaves)
        {
            foreach (var monster in wave.monsters)
            {
                mobCount += monster.count;
                string path = $"Monsters/{monster.type}";

                GameObject prefab = Resources.Load<GameObject>(path);
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
        Debug.Log($"Total monsters: {mobCount}");
    }

    private IEnumerator SpawnNPCs()
    {
        foreach (var wave in levelConfig.monsterWaves)
        {
            yield return new WaitForSeconds(wave.spawnTime);

            foreach (var monster in wave.monsters)
            {
                string path = $"Monsters/{monster.type}";
                Debug.Log($"Attempting to load prefab at path: {path}");
                GameObject prefab = Resources.Load<GameObject>(path);
                if (prefab == null)
                {
                    Debug.LogError($"Prefab for {monster.type} not found!");
                    continue;
                }

                for (int i = 0; i < monster.count; i++)
                {
                    // Randomly select a spawn point from the list of possible spawn points
                    string randomSpawnPointName = monster.spawnPoints[UnityEngine.Random.Range(0, monster.spawnPoints.Length)];

                    if (!spawnPoints.ContainsKey(randomSpawnPointName))
                    {
                        Debug.LogError($"Spawn point {randomSpawnPointName} not found!");
                        continue;
                    }

                    Transform spawnPoint = spawnPoints[randomSpawnPointName];

                    GameObject instance = objectPool.GetPooledObject(prefab);
                    if (instance != null)
                    {
                        // Update the monster's stats
                        NPCBehavior behavior = instance.GetComponent<NPCBehavior>();
                        if (behavior != null)
                        {
                            behavior.UpdateNPC(monster.healthModifier, monster.speedModifier, monster.attackDistanceModifier, monster.damageModifier);
                        }

                        // Spawn the monster near the selected point
                        Vector2 spawnPosition = GetRandomPositionNearPoint(spawnPoint, 2f); // 2f = +/- 2 blocks
                        instance.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
                        instance.SetActive(true);
                    }

                    yield return new WaitForSeconds(0.1f); // Delay between spawns
                }
            }
        }
    }


    public void OnNPCKilled()
    {
        mobsKilled += 1;
        Debug.Log($"Mobs killed: {mobsKilled}");
        Debug.Log($"Monster count: {mobCount}");
        if (mobsKilled >= mobCount)
        {
            OnWin?.Invoke(true);
        }
    }
}
