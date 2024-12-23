using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Rendering;

public class ShootingNPC : MonoBehaviour
{
    public Transform firePoint;
    public ObjectPool poolManager;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float fireRate = 1f; // This can be dynamically changed
    private float nextFireTime = 0f;
    public RangedNPC rangedNPC;
    // public Transform npcPos;

    void Awake()
    {
        if (poolManager == null)
        {
            poolManager = FindObjectOfType<ObjectPool>();
            if (poolManager == null)
            {
                Debug.LogError("No ObjectPool found in the scene. Please ensure there is an ObjectPool in the scene.");
            }
        }
        // PlayerController playerController = FindObjectOfType<PlayerController>();
        // if (playerController != null)
        // {
        //     npcPos = playerController.transform;
        // }
        // else
        // {
        //     Debug.LogError("PlayerController not found in the scene!");
        // }
    }
    void Start()
    {
        poolManager.IncreasePoolSize(bulletPrefab, GetBulletPoolSize(fireRate));
        rangedNPC = GetComponent<RangedNPC>();
        // npcPos = GetComponent<Transform>();
    }

    public void SetFireRate(float newFireRate)
    {
        float oldFireRate = fireRate;
        fireRate = newFireRate;

        int poolSizeDifference = GetBulletPoolSize(oldFireRate) - GetBulletPoolSize(fireRate);

        if (poolSizeDifference > 0)
        {
            poolManager.IncreasePoolSize(bulletPrefab, poolSizeDifference);
        }
        else if (poolSizeDifference < 0)
        {
            poolManager.DecreasePoolSize(bulletPrefab, -poolSizeDifference);
        }
    }

    private int GetBulletPoolSize(float FR)
    {
        float maxSeconds = bulletPrefab.GetComponent<NPCBullet>().TTL;
        int maxBulletsInSeconds = Mathf.CeilToInt(maxSeconds / FR);
        return maxBulletsInSeconds;
    }

    public void Fire()
    {
        GameObject bullet = poolManager.GetPooledObject(bulletPrefab);
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            bullet.SetActive(true);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Transform playerTransform = rangedNPC.getPlayer();

            Vector2 direction = (playerTransform.position - firePoint.position).normalized;
            rb.velocity = direction * bulletForce;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}