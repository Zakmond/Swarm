using System.Collections;
using UnityEngine;

public class ShootingGrenadeLauncher : MonoBehaviour
{
    public Transform firePoint;               // Where the grenade is launched from
    public GameObject grenadePrefab;          // Grenade prefab
    public ObjectPool poolManager;            // Object pool for grenades
    public Camera cam;                        // Reference to the camera for mouse aiming
    public float fireSpeed = 10f;             // Horizontal speed of the grenade
    public float maxLobHeight = 2f;           // Maximum height of the lob
    public float fireRate = 0.2f; // This can be dynamically changed
    private float nextFireTime = 0f;

    void Update()
    {
        // Check for fire input
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        // Get the target position based on the mouse cursor
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // Get a grenade from the pool
        GameObject grenade = poolManager.GetPooledObject(grenadePrefab);
        if (grenade != null)
        {
            grenade.transform.position = firePoint.position;
            grenade.transform.rotation = Quaternion.identity;
            grenade.SetActive(true);

            // Start the arc movement for the grenade
            Grenade grenadeScript = grenade.GetComponent<Grenade>();
            if (grenadeScript != null)
            {
                grenadeScript.Launch(firePoint.position, mousePos, fireSpeed, maxLobHeight);
            }
        }
    }
}
