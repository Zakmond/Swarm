using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Transform tr;
    private Animator an;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        an = GetComponent<Animator>();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            an.SetFloat("Speed", moveSpeed / 3);
            an.SetInteger("Animate", 2);
        }
        else
        {
            an.SetInteger("Animate", 0);
        }

        if (moveX != 0)
        {
            tr.localScale = new Vector3(Mathf.Sign(moveX), tr.localScale.y, tr.localScale.z);
        }

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
}