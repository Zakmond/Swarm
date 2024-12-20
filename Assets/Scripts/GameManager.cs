using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
    public float timeRemaining;
    public float maxHealthModifier;
    public float speedModifier;
    public float damageModifier;
    public float fireRateModifier;
    public float dodgeChanceModifier;
    public float maxAmmoModifier;
    public float mobsKilled = 0;
    private void Awake()
    {
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
    }
    
    void Start()
    {
        npcLevelManager = NPCLevelManager.Instance;
        playerLevelManager = PlayerLevelManager.Instance;

        characterName = playerLevelManager.playerData.character;
        gunName = playerLevelManager.playerData.weapon;
        maxHealthModifier = playerLevelManager.playerData.stats.maxHealthModifier;
        speedModifier = playerLevelManager.playerData.stats.speedModifier;
        damageModifier = playerLevelManager.playerData.stats.damageModifier;
        fireRateModifier = playerLevelManager.playerData.stats.fireRateModifier;
        dodgeChanceModifier = playerLevelManager.playerData.stats.dodgeChanceModifier;
        maxAmmoModifier = playerLevelManager.playerData.stats.maxAmmoModifier;
        level = npcLevelManager.levelConfig.levelNumber;

        playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            // Subscribe to the health change event
            playerController.OnLoss += OnResultLose;
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }

        npcLevelManager.OnWin += OnResultWin;
        npcLevelManager.OnLoss += OnResultLose;
    }

    void OnResultWin(bool result)
    {
        if (result)
        {

            levelWon = 1;
            FinishLevel();
        }
    }

    void OnResultLose(bool result)
    {
        if (result)
        {
            Debug.Log("Player Lost");
            levelWon = 0;
            FinishLevel();

        }
    }

    void FinishLevel()
    {
        mobCount = npcLevelManager.mobsKilled;
        SceneManager.LoadScene("LevelTransitionMenu");
    }
}