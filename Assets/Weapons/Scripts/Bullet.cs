using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletDamage = 50f;
    public float baseDamage = 25f;
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
    public void modifyDamage(float damageModifier)
    {
        bulletDamage = baseDamage * damageModifier;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (objectHit) return;

        RangedNPC rangedCharacter = null;

        // Check if the collided object is an NPC or RangedNPC
        if (collision.TryGetComponent<NPC>(out NPC character) || collision.TryGetComponent<RangedNPC>(out rangedCharacter))
        {
            if (character != null)
            {
                character.OnHit(bulletDamage);
            }
            else if (rangedCharacter != null)
            {
                rangedCharacter.OnHit(bulletDamage);
            }
            objectHit = true;
        }

        // Deactivate the bullet
        gameObject.SetActive(false);
    }
}
