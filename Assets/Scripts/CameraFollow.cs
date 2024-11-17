using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // The player's Transform

    void LateUpdate()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}