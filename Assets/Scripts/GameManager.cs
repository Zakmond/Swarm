using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public NPCLevelManager npcLevelManager;
    public PlayerLevelManager playerLevelManager;
    public PlayerController playerController;
    public int levelWon = 0;
    public int level = 0;
    public string characterName;
    public string gunName;
    public int mobCount;
    public PlayerStats modifiers;
    public float timeRemaining;
    public float mobsKilled = 0;
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
    }

    void Start()
    {
        npcLevelManager = NPCLevelManager.Instance;
        playerLevelManager = PlayerLevelManager.Instance;

        characterName = playerLevelManager.playerData.character;
        gunName = playerLevelManager.playerData.weapon;
        modifiers = playerLevelManager.playerData.stats;

        playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.OnLoss += OnResultLose;
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }

        npcLevelManager.OnWin += OnResultWin;
        npcLevelManager.OnLoss += OnResultLose;

        playerLevelManager.onWeaponChanged += UpdateWeapon;
    }

    void OnResultWin(bool result)
    {
        level = npcLevelManager.levelConfig.levelNumber;

        if (result)
        {

            levelWon = 1;
            FinishLevel();
        }
    }

    void UpdateWeapon(string weaponName)
    {
        gunName = weaponName;
    }

    void OnResultLose(bool result)
    {
        level = npcLevelManager.levelConfig.levelNumber;

        if (result)
        {
            Debug.Log("Player Lost");
            levelWon = 0;
            FinishLevel();

        }
    }

    public void OnLevelStartNPC()
    {
        levelWon = 0;
        npcLevelManager.OnWin += OnResultWin;
        npcLevelManager.OnLoss += OnResultLose;
    }
    public void OnLevelStartPlayer()
    {
        playerController.OnLoss += OnResultLose;
    }

    void FinishLevel()
    {
        mobCount = npcLevelManager.mobsKilled;
        timeRemaining = npcLevelManager.levelTimer;
        SceneManager.LoadScene("LevelTransitionMenu");
    }
}