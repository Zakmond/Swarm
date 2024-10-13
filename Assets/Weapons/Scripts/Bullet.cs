using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f; // Duration after which the bullet should deactivate

    void OnEnable()
    {
        // Start the deactivation coroutine when the bullet is activated
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        // Wait for the specified lifetime before deactivating
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check the object the bullet collided with
        GameObject collidedObject = collision.gameObject;

        // Optionally use the collided object for additional logic here
        Debug.Log("Bullet collided with: " + collidedObject.name);

        // Deactivate the bullet
        gameObject.SetActive(false);
    }
}