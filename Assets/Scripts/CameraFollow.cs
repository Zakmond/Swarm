using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // The player's Transform
    public Vector3 offset; // Offset between the camera and the player

    void Start()
    {
        // Calculate initial offset if not set
        if (offset == Vector3.zero)
        {
            offset = transform.position - playerTransform.position;
        }
    }

    void LateUpdate()
    {
        // Follow the player by updating the camera's position
        transform.position = playerTransform.position + offset;
    }
}