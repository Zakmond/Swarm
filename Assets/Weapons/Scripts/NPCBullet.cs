using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBullet : MonoBehaviour
{
    public float bulletDamage = 50f;
    public int TTL = 3;  // Time to live in seconds
    private bool objectHit = false;

    void OnEnable()
    {
        // disable the hit flag if this is a reused object
        objectHit = false;
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        // wait for the specified lifetime before deactivating 
        yield return new WaitForSeconds(TTL);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (objectHit) return;
        // check if the collided object is the main character
        if (collision.TryGetComponent<PlayerController>(out var mainCharacter))
        {
            mainCharacter.OnHit(bulletDamage);
            objectHit = true;
            // Deactivate the bullet
            gameObject.SetActive(false);
        }
        // Allow the bullet to pass through other NPCs and bullets
        else if (!collision.TryGetComponent<NPC>(out _) && !collision.TryGetComponent<NPCBullet>(out _))
        {
            // Deactivate the bullet if it hits any other object
            gameObject.SetActive(false);
        }
    }
}