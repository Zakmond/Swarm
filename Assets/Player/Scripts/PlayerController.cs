using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _maxHealth = 100f;
    // this can be changed over the course of the game to provide extra _health 
    private float _health = 100f;
    // player's current _health
    private float _speed = 7f;
    private float _speedModifier = 1f;
    private float _dodgeChance = 0.1f;
    private float _maxAmmoModifier = 1f;
    private float _damageModifier = 1f;
    private float _fireRateModifier = 1f;
    private Rigidbody2D rb;
    private Transform tr;
    public Camera cam;

    private Animator an;
    private DamageFlash damageFlash;
    public event Action<float> OnHealthChanged;
    public GameManager gameManager;
    public event Action<bool> OnLoss;
    public event Action<bool> OnDodge;
    private float dashSpeed = 15f; // Speed of the dash
    private float dashDuration = 0.5f; // Duration of the dash
    private bool isDashing = false; // Flag to prevent other actions during dash
    private Vector2 dashDirection;
    public GameObject dustPrefab; // Assign the dust prefab in the Inspector
    // Cooldown variables
    private float dashCooldown = 1.5f; // Cooldown duration in seconds
    private float dashCooldownTimer = 0f; // Timer to track cooldown
    public WeaponBase weaponBase;
    private PlayerStats playerStats;

    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.playerController = this;
        gameManager.OnLevelStartPlayer();
        
        damageFlash = GetComponent<DamageFlash>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        an = GetComponent<Animator>();
        cam = Camera.main;

        weaponBase = GetComponentInChildren<WeaponBase>();


        UpdateNPC(playerStats.maxHealthModifier, playerStats.speedModifier, playerStats.dodgeChanceModifier);
        UpdateWeapon(playerStats.damageModifier, playerStats.fireRateModifier, playerStats.maxAmmoModifier);

    }
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (!isDashing)
        {
            MovePlayer(moveX, moveY);
            AnimatePlayer(moveX, moveY);
        }

        if (Input.GetMouseButton(0))
        {
            an.SetBool("isShooting", true);
        }
        else
        {
            an.SetBool("isShooting", false);
        }

        // Handle cooldown timer
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Check for dash input and cooldown
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownTimer <= 0)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        isDashing = true;

        // Calculate dash direction based on keyboard input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        dashDirection = new Vector2(moveX, moveY).normalized;

        // Ensure there's a valid direction to dash
        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(an.GetFloat("DirectionX"), an.GetFloat("DirectionY")).normalized;
        }

        // Set animator parameters for the dash
        an.SetFloat("DirectionX", dashDirection.x);
        an.SetFloat("DirectionY", dashDirection.y);
        an.SetTrigger("Dash");

        SpawnDust(dashDirection);

        // Start cooldown
        dashCooldownTimer = dashCooldown;

        // Start dash coroutine
        StartCoroutine(DashCoroutine());
    }
    void SpawnDust(Vector2 direction)
    {
        if (dustPrefab != null)
        {
            // Instantiate the dust prefab
            GameObject dust = Instantiate(dustPrefab, tr.position, Quaternion.identity);

            // Set the dust object's animator direction
            Animator dustAnimator = dust.GetComponent<Animator>();
            if (dustAnimator != null)
            {
                dustAnimator.SetFloat("DirectionX", direction.x);
                dustAnimator.SetFloat("DirectionY", direction.y);
            }
        }
    }
    private System.Collections.IEnumerator DashCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            rb.velocity = dashDirection * dashSpeed * _speedModifier;
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isDashing = false;
    }
    void MovePlayer(float moveX, float moveY)
    {


        Vector2 movement = new Vector2(moveX, moveY);

        if (movement != Vector2.zero)
        {
            rb.velocity = movement.normalized * _speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void AnimatePlayer(float moveX, float moveY)
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (mousePos - (Vector2)tr.position).normalized;
        an.SetFloat("DirectionX", lookDir.x);
        an.SetFloat("DirectionY", lookDir.y);
        an.SetBool("isMoving", moveX != 0 || moveY != 0);
    }

    public void OnHit(float bulletDamage)
    {

        if (UnityEngine.Random.value < _dodgeChance)
        {
            Debug.Log("Dodge");
            OnDodge?.Invoke(true);
            return;
        }

        if (damageFlash != null)
        {
            damageFlash.CallFlash();
        }
        _health -= bulletDamage;
        OnHealthChanged?.Invoke(GetHealthPercentage());

        if (_health <= 0)
        {

            OnLoss?.Invoke(true);
        }
    }

    public float GetHealthPercentage()
    {
        return _health / _maxHealth;
    }

    public void UpdateNPC(float maxHealthModifier, float speedModifier, float dodgeChanceModifier)
    {
        _maxHealth *= maxHealthModifier;
        _speed *= speedModifier;
        _speedModifier = speedModifier;
        _dodgeChance *= dodgeChanceModifier;
    }

    public void UpdateWeapon(float damageModifier, float fireRateModifier, float maxAmmoModifier)
    {
        weaponBase.SetFireRate(fireRateModifier);
        weaponBase.SetDamageModifier(damageModifier);
        weaponBase.SetMaxAmmo(maxAmmoModifier);
    }

    public void SetPlayerStats(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
    }
}