using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class RangedNPC : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float attackDistance = 10.0f;
    [SerializeField] public float separationDistance = 0.2f;  // Distance to maintain between NPCs
    [SerializeField] public LayerMask npcLayer;
    [SerializeField] private float targetOffsetRadius = 0.1f;  // Radius for offsetting target positions
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private bool canMove = true;
    private DamageFlash damageFlash;
    private Animator animator;


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
        if (!canMove) return;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackDistance)
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
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + moveDirection, speed * Time.deltaTime);


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

        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnAttackHit()
    {
        // Check if the player is still within attack range when the hit happens
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackDistance)
        {
            // Apply damage to the player if they are still within range
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();  // Assume there's a PlayerHealth component
            if (playerHealth != null)
            {
                playerHealth.OnHit(attackDamage);
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
        return attackDistance;
    }
}
