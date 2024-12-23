using UnityEngine;


public class ShootingRifle : WeaponBase
{
    public float bulletForce = 20f;
    protected override void Fire()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audioSource.clip);
        GameObject bulletObject = poolManager.GetPooledObject(bulletPrefab);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.modifyDamage(damageModifier);
        if (bulletObject != null)
        {
            bulletObject.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            bulletObject.SetActive(true);

            Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector2 lookDir = mousePos - (Vector2)firePoint.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            bulletObject.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            rb.velocity = lookDir.normalized * bulletForce;
        }
    }
}