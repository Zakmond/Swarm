using System.Collections;
using UnityEngine;

public class ShootingGrenadeLauncher : MonoBehaviour, IShooting
{
    public Transform firePoint;               // Where the grenade is launched from
    public GameObject grenadePrefab;          // Grenade prefab
    public ObjectPool poolManager;            // Object pool for grenades
    public Camera cam;                        // Reference to the camera for mouse aiming
    public float fireSpeed = 10f;             // Horizontal speed of the grenade
    public float maxLobHeight = 2f;           // Maximum height of the lob
    public float fireRate = 0.2f;             // This can be dynamically changed
    public float damageModifier = 1f;
    private float nextFireTime = 0f;
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
    void Update()
    {
        // Check for fire input
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }
    public void setDamageModifier(float newDamageModifier)
    {
        damageModifier = newDamageModifier;
    }
    public void setFireRate(float fireRate)
    {

    }

    void Fire()
    {
        // Get the target position based on the mouse cursor
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // Get a grenade from the pool
        GameObject grenade = poolManager.GetPooledObject(grenadePrefab);
        GrenadeBullet grenadeBullet = grenade.GetComponent<GrenadeBullet>();
        grenadeBullet.ModifyDamage(damageModifier);

        if (grenade != null)
        {
            grenade.transform.position = firePoint.position;
            grenade.transform.rotation = Quaternion.identity;
            grenade.SetActive(true);

            // Start the arc movement for the grenade
            GrenadeBullet grenadeScript = grenade.GetComponent<GrenadeBullet>();
            if (grenadeScript != null)
            {
                grenadeScript.Launch(firePoint.position, mousePos, fireSpeed, maxLobHeight);
            }
        }
    }
}
