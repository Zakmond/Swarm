using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class RangedNPC : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private Transform player;
    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _attackDistance = 15.0f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private bool _canMove = true;
    private DamageFlash damageFlash;
    private Animator animator;
    [SerializeField] public float separationDistance = 0.2f;  // Distance to maintain between NPCs
    [SerializeField] public float targetOffsetRadius = 0.1f;  // Radius for offsetting target positions
    [SerializeField] public LayerMask npcLayer;
    public NPCLevelManager npcLevelManager;
    public int reward = 0;
    public void UpdateNPC(float health, float speed, float attackDistance, float attackDamage)
    {
        _health *= health;
        _speed *= speed;
        _attackDistance *= attackDistance;
        _attackDamage *= attackDamage;
    }

    private void Start()
    {
        damageFlash = GetComponent<DamageFlash>();
        animator = GetComponent<Animator>();
        PlayerController playerController = FindObjectOfType<PlayerController>();
        npcLevelManager = NPCLevelManager.Instance;

        if (playerController != null)
        {
            player = playerController.transform;
        }
    }

    private void Update()
    {
        // Flip NPC based on its movement direction
        float playerXPos = player.position.x;
        float NPCXPos = transform.position.x;
        if (playerXPos > NPCXPos)
        {
            transform.localScale = new Vector3(1, 1, 1);  
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); 
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
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 separationForce = GetSeparationForce();

        Vector2 offset = GetOffsetPosition();
        Vector2 targetPosition = (Vector2)player.position + offset;

        Vector2 moveDirection = ((targetPosition - (Vector2)transform.position).normalized + separationForce).normalized;

        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + moveDirection, _speed * Time.deltaTime);


    }

    private Vector2 GetSeparationForce()
    {
        Vector2 force = Vector2.zero;
        Collider2D[] nearbyNPCs = Physics2D.OverlapCircleAll(transform.position, separationDistance, npcLayer);

        foreach (Collider2D npc in nearbyNPCs)
        {
            if (npc.gameObject != this.gameObject) 
            {
                Vector2 diff = (Vector2)(transform.position - npc.transform.position);
                force += diff.normalized / diff.magnitude;  
            }
        }

        return force;
    }

    private Vector2 GetOffsetPosition()
    {
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
            npcLevelManager.OnNPCKilled(reward);


        }
    }

    private void OnAttackHit()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= _attackDistance)
        {
            PlayerController playerHealth = player.GetComponent<PlayerController>();  
            if (playerHealth != null)
            {
                playerHealth.OnHit(_attackDamage);
            }
        }
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
