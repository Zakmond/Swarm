using System.Collections;
using UnityEngine;

public class GrenadeBullet : MonoBehaviour
{
    public float explosionDamage = 35f;
    public float baseDamage = 35f;
    public float explosionRadius = 1f;

    private Vector2 startPosition;   // Where the grenade starts
    private Vector2 targetPosition;  // Where the grenade is aimed
    private float travelTime;        // How long the grenade will take to reach the target
    private float timer;             // Timer to track the grenade's progress
    private float maxLobHeight;      // Maximum height of the arc
    public int TTL = 5;  // Time to live in seconds
    private bool isLaunched = false; // Whether the grenade is currently in motion

    [SerializeField] private LayerMask damageableLayer;
    public void Launch(Vector2 start, Vector2 target, float speed, float lobHeight)
    {
        startPosition = start;
        targetPosition = target;
        maxLobHeight = lobHeight;

        // Calculate the travel time based on the distance and speed
        float distance = Vector2.Distance(startPosition, targetPosition);
        Debug.Log(distance);
        travelTime = distance / speed;

        timer = 0f;        // Reset the timer
        isLaunched = true; // Start the grenade movement
    }

    void Update()
    {
        if (isLaunched)
        {
            timer += Time.deltaTime;

            // Calculate progress (0 to 1) based on time
            float progress = Mathf.Clamp01(timer / travelTime);

            // Interpolate horizontal position (linear movement)
            Vector2 horizontalPosition = Vector2.Lerp(startPosition, targetPosition, progress);

            // Calculate the distance between start and target positions
            float distance = Vector2.Distance(startPosition, targetPosition);

            // Adjust the height based on the distance
            float adjustedMaxLobHeight = maxLobHeight * (distance / (distance + 1));

            // Calculate vertical (lob) position using a parabolic curve
            float height = Mathf.Sin(progress * Mathf.PI) * adjustedMaxLobHeight;

            // Combine horizontal and vertical positions
            transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y + height, 0);

            // Check if the grenade has reached its destination
            if (progress >= 1f)
            {
                OnHitGround();
            }
        }
    }
    public void modifyDamage(float damageModifier)
    {
        explosionDamage = baseDamage * damageModifier;
    }
    private void OnHitGround()
    {
        isLaunched = false; // Stop the grenade movement
        // Debug.Log("Grenade hit the ground!");
        // OnDrawGizmosSelected();

        Explode();
        // Trigger any effects (e.g., explosion) here
        // For now, just deactivate the grenade
        gameObject.SetActive(false);
    }

    private void Explode()
    {
        ExplosionEffect();
        // Debug.Log("Explosion triggered!");

        // Detect all colliders in the explosion radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageableLayer);

        foreach (Collider2D hit in hits)
        {
            // Try to get the IDamageable component on each object
            IDamageable damageable = hit.GetComponent<IDamageable>();
            // Apply damage
            damageable?.OnHit(explosionDamage);
        }

        // Optional: Add visual or sound effects for the explosion
        // Debug.Log("Explosion affected " + hits.Length + " objects.");
    }
    private void ExplosionEffect()
    {
        GameObject explosionEffect = Instantiate(Resources.Load<GameObject>("Weapons/Explosion/GrenadeExplosion"), transform.position, Quaternion.identity);

        // Scale the explosion to match the radius
        explosionEffect.transform.localScale = Vector3.one;

    }
}
