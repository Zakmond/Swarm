using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System; // Required for SceneManager

public class PlayerLevelManager : MonoBehaviour
{
    public static PlayerLevelManager Instance;

    public PlayerData playerData;

    private string filePath;

    public Vector3 playerSpawnPoint = Vector3.zero; // Assign the player spawn point in the scene
    public string playerPrefabsPath = "Players"; // Folder under Resources where player prefabs are stored
    public string weaponPrefabsPath = "Weapons"; // Folder under Resources where weapon prefabs are stored

    private GameObject currentPlayerInstance; // To keep track of the spawned player
    [SerializeField] private string DEFAULT_WEAPON = "RifleHolder";
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
        GeneratePlayerData();

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void GeneratePlayerData()
    {

        playerData = new PlayerData
        {
            character = "none", // Default character will be set in SpawnPlayer
            weapon = DEFAULT_WEAPON,
            stats = new PlayerStats
            {
                maxHealthModifier = 1.0f,
                speedModifier = 1.0f,
                fireRateModifier = 1.0f,
                dodgeChanceModifier = 1.0f,
                maxAmmoModifier = 1.0f,
                damageModifier = 1.0f
            },
            currency = 0
        };

    }

    public void UpdateCharacter(string characterName)
    {
        playerData.character = characterName;
    }

    public void UpdateWeapon(string weaponName)
    {
        playerData.weapon = weaponName;
    }

    public void UpdateStats(PlayerStats newStats)
    {
        playerData.stats = newStats;
    }
    public void UpdateCurrency(int amount)
    {
        playerData.currency += amount;
    }
    public int getCurrency()
    {
        return playerData.currency;
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
        return sceneName.StartsWith("level"); // Adjust this to match your naming convention
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
        playerData.character = characterToLoad;
        GameObject playerPrefab = Resources.Load<GameObject>($"{playerPrefabsPath}/{characterToLoad}");
        if (playerPrefab == null)
        {
            Debug.LogError($"Player prefab for {characterToLoad} not found!");
            return;
        }

        // Spawn the player
        currentPlayerInstance = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);

        // Apply stats to the player (if necessary)


        Debug.Log($"Player {characterToLoad} spawned.");

        // Notify the camera about the new player
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.SetPlayerTransform(currentPlayerInstance.transform);
        }

        // Spawn and attach the weapon
        SpawnWeapon();

        PlayerBehavior playerBehavior = currentPlayerInstance.GetComponent<PlayerBehavior>();
        if (playerBehavior != null)
        {
            playerBehavior.UpdateNPC(
                playerData.stats
            );
        }
    }

    private void SpawnWeapon()
    {
        if (currentPlayerInstance == null)
        {
            Debug.LogError("No player instance found to attach the weapon!");
            return;
        }

        // Load the weapon prefab
        string weaponToLoad = playerData.weapon == "none" ? DEFAULT_WEAPON : playerData.weapon;
        playerData.weapon = weaponToLoad;
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
    public float damageModifier;
}
