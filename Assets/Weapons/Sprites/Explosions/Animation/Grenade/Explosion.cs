using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void DestroyExplosion()
    {
        // Destroy the explosion GameObject
        Destroy(gameObject);
    }
}