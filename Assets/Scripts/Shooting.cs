using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Rendering;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPreFab;
    public float bulletForce = 20f;
    public Camera cam;
    Vector2 mousePos;
    public float fireRate = 0.5f; // Time in seconds between each shot
    private float nextFireTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPreFab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - (Vector2)firePoint.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        rb.AddForce(lookDir.normalized * bulletForce, ForceMode2D.Impulse);

        Debug.Log(angle);
    }
}
