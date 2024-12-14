using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // The player's Transform

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            // Follow the player's position
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }
    }

    public void SetPlayerTransform(Transform newPlayerTransform)
    {
        playerTransform = newPlayerTransform;
    }
}
