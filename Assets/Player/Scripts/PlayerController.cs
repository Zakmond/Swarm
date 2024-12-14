using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _maxHealth = 100f;
    // this can be changed over the course of the game to provide extra _health 
    private float _health = 100f;
    // player's current _health
    private float _speed = 7f;
    private float _fireRate = 0.5f;
    private float _dodgeChance = 0f;
    private int _maxAmmo = 30;
    private Rigidbody2D rb;
    private Transform tr;
    public Camera cam;

    private Animator an;
    private DamageFlash damageFlash;

    void Start()
    {
        damageFlash = GetComponent<DamageFlash>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        an = GetComponent<Animator>();
        cam = Camera.main;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        MovePlayer(moveX, moveY);
        AnimatePlayer(moveX, moveY);

        if (Input.GetMouseButton(0))
        {
            an.SetBool("isShooting", true);

        }
        else
        {
            an.SetBool("isShooting", false);
        }
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
        if (damageFlash != null)
        {
            damageFlash.CallFlash();
        }
        _health -= bulletDamage;
        Debug.Log("Player hit");
    }

    public void UpdateNPC(float maxHealthModifier, float speedModifier, float fireRateModifier, float dodgeChanceModifier, float maxAmmoModifier)
    {
        _maxHealth *= maxHealthModifier;
        _speed *= speedModifier;
        _fireRate *= fireRateModifier;
        _dodgeChance *= dodgeChanceModifier;
        _maxAmmo = Mathf.RoundToInt(_maxAmmo * maxAmmoModifier);
    }
}