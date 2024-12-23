using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] public float _maxHealthModifier = 1f;
    [SerializeField] public float _speedModifier = 1f;
    [SerializeField] public float _fireRateModifier = 1f;
    [SerializeField] public float _dodgeChanceModifier = 1f;
    [SerializeField] public float _maxAmmoModifier = 1f;
    [SerializeField] public float _damageModifier = 1f;


    private PlayerController playerComponent;

    private void Awake()
    {
        playerComponent = GetComponent<PlayerController>();
        if (playerComponent == null)
        {
            Debug.LogError("PlayerController component not found on PlayerBehavior's GameObject!");
        }
    }

    private void Start()
    {
        playerComponent.UpdateNPC(_maxHealthModifier, _speedModifier, _dodgeChanceModifier);
    }

    public void UpdateNPC(PlayerStats playerStats)
    {
        _maxHealthModifier = playerStats.maxHealthModifier;
        _speedModifier = playerStats.speedModifier;
        _dodgeChanceModifier = playerStats.dodgeChanceModifier;
        _fireRateModifier = playerStats.fireRateModifier;
        _maxAmmoModifier = playerStats.maxAmmoModifier;
        _damageModifier = playerStats.damageModifier;
        playerComponent.SetPlayerStats(playerStats);
    }
}
