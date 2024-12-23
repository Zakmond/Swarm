using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; 

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }
    }

    public void SetPlayerTransform(Transform newPlayerTransform)
    {
        playerTransform = newPlayerTransform;
    }
}
