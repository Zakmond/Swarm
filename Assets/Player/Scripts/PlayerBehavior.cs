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
        // Initialize playerComponent in Awake so it’s ready before UpdateNPC is called
        playerComponent = GetComponent<PlayerController>();
        if (playerComponent == null)
        {
            Debug.LogError("PlayerController component not found on PlayerBehavior's GameObject!");
        }
    }

    private void Start()
    {
        // Apply modifiers during Start
        playerComponent.UpdateNPC(_maxHealthModifier, _speedModifier, _dodgeChanceModifier);
    }

    public void UpdateNPC(float maxHealthModifier, float speedModifier, float fireRateModifier, float dodgeChanceModifier, float maxAmmoModifier, float damageModifier)
    {
        _maxHealthModifier = maxHealthModifier;
        _speedModifier = speedModifier;
        _dodgeChanceModifier = dodgeChanceModifier;
        _fireRateModifier = fireRateModifier;
        _maxAmmoModifier = maxAmmoModifier;
        _damageModifier = damageModifier;
        playerComponent.UpdateNPC(_maxHealthModifier, _speedModifier, _dodgeChanceModifier);
        playerComponent.UpdateWeapon(_damageModifier, _fireRateModifier, _maxAmmoModifier);
    }
}
