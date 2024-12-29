using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System; // Required for SceneManager

public class PlayerLevelManager : MonoBehaviour
{
    public static PlayerLevelManager Instance;

    public PlayerData playerData;

    private string filePath;

    public Vector3 playerSpawnPoint = Vector3.zero;
    public string playerPrefabsPath = "Players";
    public string weaponPrefabsPath = "Weapons";

    private GameObject currentPlayerInstance;
    [SerializeField] private string DEFAULT_WEAPON = "RifleHolder";

    private bool firstTimeLoad = true;
    public event Action<string> onWeaponChanged;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        filePath = Path.Combine(Application.persistentDataPath, "playerConfig.json");

        GeneratePlayerData();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void GeneratePlayerData()
    {

        playerData = new PlayerData
        {
            character = "none",
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
        onWeaponChanged?.Invoke(weaponName);
    }

    public void UpdateStats(PlayerStats newStats)
    {
        playerData.stats = newStats;
    }
    public void UpdateHealthModifier(float modifier)
    {
        playerData.stats.maxHealthModifier += modifier;
    }
    public void UpdateSpeedModifier(float modifier)
    {
        playerData.stats.speedModifier += modifier;
    }
    public void UpdateFireRateModifier(float modifier)
    {
        playerData.stats.fireRateModifier += modifier;
    }
    public void UpdateDodgeChanceModifier(float modifier)
    {
        playerData.stats.dodgeChanceModifier += modifier;
    }
    public void UpdateMaxAmmoModifier(float modifier)
    {
        playerData.stats.maxAmmoModifier += modifier;
    }
    public void UpdateDamageModifier(float modifier)
    {
        playerData.stats.damageModifier += modifier;
    }
    public void UpdateCurrency(int amount)
    {
        playerData.currency += amount;
    }
    public int GetCurrency()
    {
        return playerData.currency;
    }
    public void SubTrackCurrency(int amount)
    {
        playerData.currency -= amount;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsGameplayScene(scene.name))
        {
            SpawnPlayer();
        }
    }

    private bool IsGameplayScene(string sceneName)
    {
        return sceneName.StartsWith("level");
    }

    public void SpawnPlayer()
    {
        if (currentPlayerInstance != null)
        {
            Destroy(currentPlayerInstance);
        }

        string characterToLoad = playerData.character == "none" ? "Ares" : playerData.character;

        if (firstTimeLoad)
        {
            firstTimeLoad = false;
            if (characterToLoad == "Ares")
            {
                playerData.stats.maxHealthModifier = 1.0f;
                playerData.stats.speedModifier = 0.95f;
                playerData.stats.fireRateModifier = 0.95f;
                playerData.stats.dodgeChanceModifier = 1.0f;
                playerData.stats.maxAmmoModifier = 1.1f;
                playerData.stats.damageModifier = 1.1f;
            }
            else if (characterToLoad == "Nox")
            {
                playerData.stats.maxHealthModifier = 1.0f;
                playerData.stats.speedModifier = 1.05f;
                playerData.stats.fireRateModifier = 1.05f;
                playerData.stats.dodgeChanceModifier = 1.05f;
                playerData.stats.maxAmmoModifier = 1.0f;
                playerData.stats.damageModifier = 0.95f;
            }
        }
        playerData.character = characterToLoad;
        GameObject playerPrefab = Resources.Load<GameObject>($"{playerPrefabsPath}/{characterToLoad}");
        if (playerPrefab == null)
        {
            Debug.LogError($"Player prefab for {characterToLoad} not found!");
            return;
        }

        currentPlayerInstance = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);



        Debug.Log($"Player {characterToLoad} spawned.");

        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.SetPlayerTransform(currentPlayerInstance.transform);
        }

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

        string weaponToLoad = playerData.weapon == "none" ? DEFAULT_WEAPON : playerData.weapon;
        playerData.weapon = weaponToLoad;
        GameObject weaponPrefab = Resources.Load<GameObject>($"{weaponPrefabsPath}/{weaponToLoad}");
        if (weaponPrefab == null)
        {
            Debug.LogError($"Weapon prefab for {weaponToLoad} not found in Resources!");
            return;
        }

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
