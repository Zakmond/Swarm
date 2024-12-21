using UnityEngine;

public class ShootingGrenadeLauncher : WeaponBase
{
    public float fireSpeed = 10f;             // Horizontal speed of the grenade
    public float maxLobHeight = 2f;           // Maximum height of the lob

    protected override void Fire()
    {
        // Get the target position based on the mouse cursor
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // Get a grenade from the pool
        GameObject grenade = poolManager.GetPooledObject(bulletPrefab);
        GrenadeBullet grenadeBullet = grenade.GetComponent<GrenadeBullet>();
        grenadeBullet.modifyDamage(damageModifier);

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
