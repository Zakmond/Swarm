using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private Transform player;
    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _attackDistance = 1.0f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private bool _canMove = true;
    private DamageFlash _damageFlash;
    private Animator _animator;
    [SerializeField] public float separationDistance = 0.2f;  // Distance to maintain between NPCs
    [SerializeField] public float targetOffsetRadius = 0.1f;  // Radius for offsetting target positions
    [SerializeField] public LayerMask npcLayer;


    public void UpdateNPC(float healthModifier, float speedModifier, float attackDistanceModifier, float attackDamageModifier)
    {
        _health *= healthModifier;
        _speed *= speedModifier;
        _attackDistance *= attackDistanceModifier;
        _attackDamage *= attackDamageModifier;
    }

    private void Start()
    {
        _damageFlash = GetComponent<DamageFlash>();
        _animator = GetComponent<Animator>();
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
    }

    private void Update()
    {
        FollowAndAttackPlayer();
    }

    private void FollowAndAttackPlayer()
    {
        if (!_canMove) return;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > _attackDistance)
        {
            MoveTowardsPlayer();
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
            _animator.SetTrigger("attack");
        }
    }

    // Modified MoveTowardsPlayer to add separation and target offsetting
    private void MoveTowardsPlayer()
    {
        // Calculate the separation force to avoid NPCs stacking
        Vector2 separationForce = GetSeparationForce();

        // Calculate the offset target position to avoid all NPCs targeting the exact same point
        Vector2 offset = GetOffsetPosition();
        Vector2 targetPosition = (Vector2)player.position + offset;

        // Combine the direction towards the player and the separation force
        Vector2 moveDirection = ((targetPosition - (Vector2)transform.position).normalized + separationForce).normalized;

        // Move the NPC using the combined force
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + moveDirection, _speed * Time.deltaTime);

        // Flip NPC based on its movement direction
        float playerXPos = player.position.x;
        float NPCXPos = transform.position.x;
        if (playerXPos > NPCXPos)
        {
            transform.localScale = new Vector3(1, 1, 1);  // Face right
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // Face left
        }
    }

    // Calculate separation force to prevent NPCs from stacking
    private Vector2 GetSeparationForce()
    {
        Vector2 force = Vector2.zero;
        Collider2D[] nearbyNPCs = Physics2D.OverlapCircleAll(transform.position, separationDistance, npcLayer);

        foreach (Collider2D npc in nearbyNPCs)
        {
            if (npc.gameObject != this.gameObject)  // Ignore self
            {
                Vector2 diff = (Vector2)(transform.position - npc.transform.position);
                force += diff.normalized / diff.magnitude;  // Force inversely proportional to distance
            }
        }

        return force;
    }

    // Get a small random offset around the target to avoid stacking at the exact same point
    private Vector2 GetOffsetPosition()
    {
        // Create a random offset within a small radius around the player
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        return randomDirection * targetOffsetRadius;
    }

    public void OnHit(float damage)
    {
        if (_damageFlash != null)
        {
            _damageFlash.CallFlash();
        }

        _health -= damage;

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnAttackHit()
    {
        // Check if the player is still within attack range when the hit happens
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= _attackDistance)
        {
            // Apply damage to the player if they are still within range
            PlayerController player_health = player.GetComponent<PlayerController>();  // Assume there's a Player_health component
            if (player_health != null)
            {
                player_health.OnHit(_attackDamage);
            }
        }
    }
}
