using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransitionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text levelSummaryLabelText;
    public TMP_Text resultLabelText;
    public TMP_Text characterLabelText;
    public TMP_Text gunLabelText;
    public TMP_Text mobCountLabelText;
    public TMP_Text timeRemainingLabelText;
    public TMP_Text nextStageLabelText;
    public List<TMP_Text> modifierPlaceholders;
    public GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;


        levelSummaryLabelText.text = "Level " + gameManager.level + " Summary";
        resultLabelText.text = gameManager.levelWon == 1 ? "Victory!" : "Defeat!";
        if (gameManager.levelWon == 1)
        {
            resultLabelText.color = Color.green;
        }
        else
        {
            resultLabelText.color = Color.red;
        }

        characterLabelText.text = gameManager.characterName;
        gunLabelText.text = gameManager.gunName;
        mobCountLabelText.text = gameManager.mobCount.ToString();
        timeRemainingLabelText.text = gameManager.timeRemaining.ToString();
        nextStageLabelText.text = gameManager.levelWon == 1 ? "Market" : "Retry";

        DisplayActiveModifiers();
    }

    private void DisplayActiveModifiers()
    {
        // List of active modifiers with their labels and values
        var activeModifiers = new List<string>();

        // Helper function to format and colorize modifiers
        static string FormatModifier(float value, string label)
        {
            Debug.Log($"Value: {value} Label: {label}");
            if (value > 1f)
            {
                return $"<color=#55BB5A>+{Mathf.RoundToInt((value - 1f) * 100)}% {label}</color>";
            }
            else if (value < 1f)
            {
                return $"<color=#D72048>-{Mathf.RoundToInt((value - 1f) * 100)}% {label}</color>";
            }
            return null; // No display for neutral modifiers
        }

        // Collect active modifiers with formatting
        string modifier;
        modifier = FormatModifier(gameManager.modifiers.maxHealthModifier, "Max Health");
        if (modifier != null) activeModifiers.Add(modifier);

        modifier = FormatModifier(gameManager.modifiers.speedModifier, "Speed");
        if (modifier != null) activeModifiers.Add(modifier);

        modifier = FormatModifier(gameManager.modifiers.damageModifier, "Damage");
        if (modifier != null) activeModifiers.Add(modifier);

        modifier = FormatModifier(gameManager.modifiers.fireRateModifier, "Fire Rate");
        if (modifier != null) activeModifiers.Add(modifier);

        modifier = FormatModifier(gameManager.modifiers.dodgeChanceModifier, "Dodge Chance");
        if (modifier != null) activeModifiers.Add(modifier);

        modifier = FormatModifier(gameManager.modifiers.maxAmmoModifier, "Max Ammo");
        if (modifier != null) activeModifiers.Add(modifier);

        // Assign active modifiers to placeholders
        for (int i = 0; i < modifierPlaceholders.Count; i++)
        {
            if (i < activeModifiers.Count)
            {
                modifierPlaceholders[i].text = activeModifiers[i];
                modifierPlaceholders[i].gameObject.SetActive(true); // Show the placeholder
            }
            else
            {
                modifierPlaceholders[i].gameObject.SetActive(false); // Hide unused placeholders
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void GoToMarket()
    {
        SceneManager.LoadScene("Market Menu");
    }
}
