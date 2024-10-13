using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Rendering;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public ObjectPool bulletPool;
    public float bulletForce = 20f;
    public Camera cam;
    public float fireRate = 0.2f; // This can be dynamically changed
    private float nextFireTime = 0f;

    void Start()
    {
        UpdateBulletPoolSize();
    }

    public void SetFireRate(float newFireRate)
    {
        fireRate = newFireRate;
        UpdateBulletPoolSize();
    }

    private void UpdateBulletPoolSize()
    {
        float maxSeconds = 2f;
        int maxBulletsInTwoSeconds = Mathf.CeilToInt(maxSeconds / fireRate);
        bulletPool.SetPoolSize(maxBulletsInTwoSeconds);
    }

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
        GameObject bullet = bulletPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.SetActive(true);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector2 lookDir = mousePos - (Vector2)firePoint.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            rb.velocity = lookDir.normalized * bulletForce;
        }
    }
}