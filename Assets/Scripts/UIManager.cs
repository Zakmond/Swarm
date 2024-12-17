using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RectTransform healthFill;          // Reference to HealthFill RectTransform
    private PlayerController playerController; // Reference to PlayerController

    void Start()
    {
        // Find PlayerController in the scene at runtime
        playerController = FindObjectOfType<PlayerController>();

        if (playerController != null)
        {
            // Subscribe to the health change event
            playerController.OnHealthChanged += UpdateHealthBar;
            // Initialize the health bar
            UpdateHealthBar(playerController.GetHealthPercentage());
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
    }

    void UpdateHealthBar(float healthPercent)
    {
        // Change the width of the health fill based on the health percentage
        float newWidth = Math.Max(0, healthPercent * 245);
        healthFill.sizeDelta = new Vector2(newWidth, healthFill.sizeDelta.y);
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (playerController != null)
        {
            playerController.OnHealthChanged -= UpdateHealthBar;
        }
    }
}
