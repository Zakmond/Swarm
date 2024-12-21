using UnityEngine;
using System;

public abstract class WeaponBase : MonoBehaviour
{
    public Transform firePoint;
    public float reloadTime = 2f;
    public event Action<float> OnReloadStarted;
    public float fireRate = 1.0f; // Fire rate in seconds
    protected float nextFireTime = 0f;
    protected float damageModifier = 1f;
    protected int ammo = 0;
    protected int maxAmmo = 15;
    public ObjectPool poolManager;
    public GameObject bulletPrefab;
    public Camera cam;

    protected abstract void Fire();
    void Awake()
    {
        // Automatically find the ObjectPool in the scene if not assigned
        if (poolManager == null)
        {
            poolManager = FindObjectOfType<ObjectPool>();
            if (poolManager == null)
            {
                Debug.LogError("No ObjectPool found in the scene. Please ensure there is an ObjectPool in the scene.");
            }
        }

        // Automatically find the Camera if not assigned
        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("No Camera tagged as 'MainCamera' found. Please tag your main camera as 'MainCamera'.");
            }
        }
    }
    protected virtual void Start()
    {
        poolManager.IncreasePoolSize(bulletPrefab, GetBulletPoolSize(fireRate));
        ammo = maxAmmo;
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }
    private void Reload()
    {
        OnReloadStarted?.Invoke(reloadTime);

        StartCoroutine(ReloadCoroutine());
    }

    private System.Collections.IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);

        Debug.Log($"{gameObject.name} has finished reloading.");
    }
    public virtual void SetFireRate(float newFireRate)
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
    protected int GetBulletPoolSize(float FR)
    {
        float maxSeconds = bulletPrefab.GetComponent<Bullet>().TTL;
        int maxBulletsInSeconds = Mathf.CeilToInt(maxSeconds / FR);
        return maxBulletsInSeconds;
    }
}
