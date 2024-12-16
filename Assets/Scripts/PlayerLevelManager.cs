using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement; // Required for SceneManager

public class PlayerLevelManager : MonoBehaviour
{
    public static PlayerLevelManager Instance;

    public PlayerData playerData;

    private string filePath;

    public Vector3 playerSpawnPoint = Vector3.zero; // Assign the player spawn point in the scene
    public string playerPrefabsPath = "Players"; // Folder under Resources where player prefabs are stored
    public string weaponPrefabsPath = "Weapons"; // Folder under Resources where weapon prefabs are stored

    private GameObject currentPlayerInstance; // To keep track of the spawned player

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

        // File path for the player configuration
        filePath = Path.Combine(Application.persistentDataPath, "playerConfig.json");

        // Load or create player data
        LoadOrGeneratePlayerData();

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void LoadOrGeneratePlayerData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Player data loaded.");
        }
        else
        {
            playerData = new PlayerData
            {
                character = "none", // Default character will be set in SpawnPlayer
                weapon = "none", // Default weapon
                stats = new PlayerStats
                {
                    maxHealthModifier = 1.0f,
                    speedModifier = 1.0f,
                    fireRateModifier = 1.0f,
                    dodgeChanceModifier = 1.0f,
                    maxAmmoModifier = 1.0f,
                },
                currency = 0
            };
            SavePlayerData();
            Debug.Log("Player data generated.");
        }
    }

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Player data saved.");
    }

    public void UpdateCharacter(string characterName)
    {
        playerData.character = characterName;
        SavePlayerData();
    }

    public void UpdateWeapon(string weaponName)
    {
        playerData.weapon = weaponName;
        SavePlayerData();
    }

    public void UpdateStats(PlayerStats newStats)
    {
        playerData.stats = newStats;
        SavePlayerData();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is a gameplay scene
        if (IsGameplayScene(scene.name))
        {
            SpawnPlayer();
        }
    }

    private bool IsGameplayScene(string sceneName)
    {
        // Check if the scene name starts with "Level"
        return sceneName.StartsWith("Level"); // Adjust this to match your naming convention
    }

    public void SpawnPlayer()
    {
        // Destroy any existing player instance before spawning a new one
        if (currentPlayerInstance != null)
        {
            Destroy(currentPlayerInstance);
        }

        // Determine the player prefab to load
        string characterToLoad = playerData.character == "none" ? "Ares" : playerData.character;

        GameObject playerPrefab = Resources.Load<GameObject>($"{playerPrefabsPath}/{characterToLoad}");
        if (playerPrefab == null)
        {
            Debug.LogError($"Player prefab for {characterToLoad} not found!");
            return;
        }

        // Spawn the player
        currentPlayerInstance = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);

        // Apply stats to the player (if necessary)
        // PlayerBehavior playerBehavior = currentPlayerInstance.GetComponent<PlayerBehavior>();
        // if (playerBehavior != null)
        // {
        //     playerBehavior.UpdateNPC(
        //         playerData.stats.maxHealthModifier,
        //         playerData.stats.speedModifier,
        //         playerData.stats.fireRateModifier,
        //         playerData.stats.dodgeChanceModifier,
        //         playerData.stats.maxAmmoModifier
        //     );
        // }

        Debug.Log($"Player {characterToLoad} spawned.");

        // Notify the camera about the new player
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.SetPlayerTransform(currentPlayerInstance.transform);
        }

        // Spawn and attach the weapon
        SpawnWeapon();
    }

    private void SpawnWeapon()
    {
        if (currentPlayerInstance == null)
        {
            Debug.LogError("No player instance found to attach the weapon!");
            return;
        }

        // Load the weapon prefab
        string weaponToLoad = playerData.weapon == "none" ? "GrenadeLauncherHolder" : playerData.weapon;
        GameObject weaponPrefab = Resources.Load<GameObject>($"{weaponPrefabsPath}/{weaponToLoad}");
        if (weaponPrefab == null)
        {
            Debug.LogError($"Weapon prefab for {weaponToLoad} not found in Resources!");
            return;
        }

        // Instantiate the weapon as a child of the player instance
        GameObject weaponInstance = Instantiate(weaponPrefab, currentPlayerInstance.transform);
        Debug.Log($"Weapon {weaponToLoad} spawned and set as a child of the player.");
    }
}




[System.Serializable]
public class PlayerData
{
    public string character;
    public string weapon;
    public PlayerStats stats;
    public int currency;
}

[System.Serializable]
public class PlayerStats
{
    public float maxHealthModifier;
    public float speedModifier;
    public float fireRateModifier;
    public float dodgeChanceModifier;
    public float maxAmmoModifier;
}
