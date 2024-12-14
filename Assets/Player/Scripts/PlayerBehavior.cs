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


    private PlayerController playerComponent;

    private void Start()
    {
        playerComponent = GetComponent<PlayerController>();

        playerComponent.UpdateNPC(_maxHealthModifier, _speedModifier, _fireRateModifier, _dodgeChanceModifier, _maxAmmoModifier);
    }

    public void UpdateNPC(float maxHealthModifier, float speedModifier, float fireRateModifier, float dodgeChanceModifier, float maxAmmoModifier)
    {
        _maxHealthModifier = maxHealthModifier;
        _speedModifier = speedModifier;
        _fireRateModifier = fireRateModifier;
        _dodgeChanceModifier = dodgeChanceModifier;
        _maxAmmoModifier = maxAmmoModifier;
        playerComponent.UpdateNPC(_maxHealthModifier, _speedModifier, _fireRateModifier, _dodgeChanceModifier, _maxAmmoModifier);
    }
}
