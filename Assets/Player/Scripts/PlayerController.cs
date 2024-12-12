using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxHealth = 100f;
    // this can be changed over the course of the game to provide extra health 
    public float health = 100f;
    // player's current health
    public float moveSpeed = 7f;
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
            rb.velocity = movement.normalized * moveSpeed;
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
        health -= bulletDamage;
        Debug.Log("Player hit");
    }
}