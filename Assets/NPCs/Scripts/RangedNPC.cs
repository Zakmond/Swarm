using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class RangedNPC : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private Transform player;
    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _attackDistance = 1.0f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private bool _canMove = true;
    private DamageFlash damageFlash;
    private Animator animator;
    [SerializeField] public float separationDistance = 0.2f;  // Distance to maintain between NPCs
    [SerializeField] public float targetOffsetRadius = 0.1f;  // Radius for offsetting target positions
    [SerializeField] public LayerMask npcLayer;
    public void UpdateNPC(float health, float speed, float attackDistance, float attackDamage)
    {
        _health = health;
        _speed = speed;
        _attackDistance = attackDistance;
        _attackDamage = attackDamage;
    }

    private void Start()
    {
        damageFlash = GetComponent<DamageFlash>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
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

        FollowAndAttackPlayer();
    }

    private void FollowAndAttackPlayer()
    {
        if (!_canMove) return;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > _attackDistance)
        {
            MoveTowardsPlayer();
            animator.SetBool("isAttacking", false);
        }
        else
        {
            animator.SetBool("isAttacking", true);
            // Fire();
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
        if (damageFlash != null)
        {
            damageFlash.CallFlash();
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
            PlayerController playerHealth = player.GetComponent<PlayerController>();  // Assume there's a PlayerHealth component
            if (playerHealth != null)
            {
                playerHealth.OnHit(_attackDamage);
            }
        }
    }

    private void Fire()
    {

    }

    public Transform getPlayer()
    {
        return player;
    }

    public float getAttackDistance()
    {
        return _attackDistance;
    }
}
