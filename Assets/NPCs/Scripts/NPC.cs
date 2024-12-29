using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour, IDamageable
{
    public float bossMaxHealth = 100f;
    [SerializeField] private float _health = 100f;
    [SerializeField] private Transform player;
    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _attackDistance = 1.0f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private bool _canMove = true;
    private DamageFlash _damageFlash;
    private Animator _animator;
    [SerializeField] public float separationDistance = 0.2f; 
    [SerializeField] public float targetOffsetRadius = 0.1f;  
    [SerializeField] public LayerMask npcLayer;
    public NPCLevelManager npcLevelManager;
    public bool isBoss = false;
    public int reward = 0;
    public bool inverse = false;
    
    public string uniqueID;
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
        npcLevelManager = NPCLevelManager.Instance;
        if (playerController != null)
        {
            player = playerController.transform;
        }

        uniqueID = Guid.NewGuid().ToString();
    }

    private void Update()
    {
        FollowAndAttackPlayer();
    }

    private void FollowAndAttackPlayer()
    {
        if (!_canMove) return;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > 0.1)
        {
            MoveTowardsPlayer();

        }
        if (distance > _attackDistance)
        {
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
            _animator.SetTrigger("attack");
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 separationForce = GetSeparationForce();

        Vector2 offset = GetOffsetPosition();
        Vector2 targetPosition = (Vector2)player.position + offset;

        Vector2 moveDirection = ((targetPosition - (Vector2)transform.position).normalized + separationForce).normalized;

        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + moveDirection, _speed * Time.deltaTime);

        float playerXPos = player.position.x;
        float NPCXPos = transform.position.x;
        if (playerXPos > NPCXPos)
        {
            transform.localScale = new Vector3(inverse ? -1 : 1, 1, 1);  // Face right
        }
        else
        {
            transform.localScale = new Vector3(inverse ? 1 : -1, 1, 1); // Face left
        }
    }

    private Vector2 GetSeparationForce()
    {
        Vector2 force = Vector2.zero;
        Collider2D[] nearbyNPCs = Physics2D.OverlapCircleAll(transform.position, separationDistance, npcLayer);

        foreach (Collider2D npc in nearbyNPCs)
        {
            if (npc.gameObject != this.gameObject)  // Ignore self
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
        if (_damageFlash != null)
        {
            _damageFlash.CallFlash();
        }

        _health -= damage;

        if (_health <= 0)
        {
            if (isBoss)
            {
                _animator.SetTrigger("death");
                _canMove = false;
            }
            else
            {
                Destroy(gameObject);
                npcLevelManager.OnNPCKilled(reward, isBoss: false, uniqueID);
            }
        }
    }

    public void OnBossDeath()
    {
        Destroy(gameObject);
        npcLevelManager.OnNPCKilled(reward, isBoss: false, uniqueID);
    }
    private void OnAttackHit()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= _attackDistance)
        {
            PlayerController player_health = player.GetComponent<PlayerController>();
            if (player_health != null)
            {
                player_health.OnHit(_attackDamage);
            }
        }
    }
}
