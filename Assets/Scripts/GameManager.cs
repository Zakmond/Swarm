using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager Instance;
    public  NPCLevelManager npcLevelManager;
    public PlayerLevelManager playerLevelManager;
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
    }

    void Update()
    {

    }
}
