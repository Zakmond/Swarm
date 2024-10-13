using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Camera cam;

    public SpriteRenderer weaponSR;
    public SpriteRenderer handsSR;
    public Transform playerPos;
    private Transform pos;

    void Start()
    {
        pos = GetComponent<Transform>();
    }

    void Update()
    {
        float scaleOffset = 0f;
        if (pos.parent.localScale.x < 0) scaleOffset = 0f;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (mousePos - (Vector2)pos.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        pos.rotation = Quaternion.Euler(0, 0, angle + scaleOffset);

        // Change the scale x of the object to match the parent's scale x
        pos.localScale = new Vector3(pos.parent.localScale.x, pos.localScale.y, pos.localScale.z);

        // Check rotation and adjust the y scale
        float zRotation = pos.rotation.eulerAngles.z;


        // Mouse direction according to player (to get accurate animation change state)
        Vector2 lookDirPlayer = (mousePos - (Vector2)playerPos.position).normalized;
        float anglePlayer = Mathf.Atan2(lookDirPlayer.y, lookDirPlayer.x) * Mathf.Rad2Deg;
        float eastToNorthEast = 22.5f; // Angle when player transitions from east to north east
        float northWestToWest = 157.5f; // Angle when player transitions from north west to west
        if (anglePlayer > eastToNorthEast && anglePlayer < northWestToWest)
        {
            // position the hands and the weapon behind the player
            weaponSR.sortingOrder = 1;
            handsSR.sortingOrder = 1;
        }
        else
        {
            // position the hands and the weapon in front of the player
            weaponSR.sortingOrder = 3;
            handsSR.sortingOrder = 3;
        }


        // Normalize the angle
        if (zRotation > 180) zRotation -= 360;

        if ((zRotation < -90 && zRotation >= -180) || (zRotation > 90 && zRotation <= 180))
        {
            pos.localScale = new Vector3(pos.localScale.x, -1, pos.localScale.z);
        }
        else
        {
            pos.localScale = new Vector3(pos.localScale.x, 1, pos.localScale.z);
        }

        if (Input.GetMouseButton(0))
        {
            weaponSR.enabled = true; // Enable rendering
            handsSR.enabled = true; // Enable rendering
        }
        else
        {
            weaponSR.enabled = false; // Disable rendering
            handsSR.enabled = false; // Disable rendering
        }
    }
}