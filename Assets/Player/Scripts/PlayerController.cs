using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Transform tr;
    public Camera cam;

    private Animator an;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        an = GetComponent<Animator>();
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
        Vector2 lookDirTest = (mousePos - (Vector2)tr.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        // Debug.Log("looKDir" + lookDir + "lookDirTest" + lookDirTest);
        // Debug.Log("looKDirTest" + lookDirTest);
        an.SetFloat("Angle", angle);
        an.SetBool("isMoving", moveX != 0 || moveY != 0);
    }

}