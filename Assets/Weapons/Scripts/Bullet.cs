using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
        // check if the collided object is an NPC
        if (collision.TryGetComponent<NPC>(out var character))
        {
            character.OnHit(bulletDamage);
            objectHit = true;
        }

        // Deactivate the bullet
        gameObject.SetActive(false);
    }
}
