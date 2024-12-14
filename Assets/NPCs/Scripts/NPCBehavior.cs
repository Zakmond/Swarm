using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    [SerializeField] public float _attackDistance = 1.0f;
    [SerializeField] public float _attackDamage = 10f;
    [SerializeField] public float _health = 100f;
    [SerializeField] public float _speed = 2.0f;
    private IDamageable npcComponent;

    private void Start()
    {
        // Try to get the NPC component
        npcComponent = GetComponent<NPC>() as IDamageable;

        // If NPC component is not found, try to get the RangedNPC component
        if (npcComponent == null)
        {
            npcComponent = GetComponent<RangedNPC>() as IDamageable;
        }

        // Check if neither component was found
        if (npcComponent == null)
        {
            Debug.LogError("Neither NPC nor RangedNPC component found on the GameObject.");
        }

        npcComponent.UpdateNPC(_health, _speed, _attackDistance, _attackDamage);
    }

    public void UpdateNPC(float health, float speed, float attackDistance, float attackDamage)
    {
        _health = health * _health;
        _speed = speed * _speed;
        _attackDistance = attackDistance * _attackDistance;
        _attackDamage = attackDamage * _attackDamage;
        npcComponent.UpdateNPC(_health, _speed, _attackDistance, _attackDamage);
    }
}
