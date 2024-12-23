using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public float bulletDamage = 35f;
    public float baseDamage = 35f;
    public int TTL = 3; 
    private bool objectHit = false;
    void OnEnable()
    {
        objectHit = false;
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
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

        gameObject.SetActive(false);
    }
}
