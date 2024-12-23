using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class LevelTransitionManager : MonoBehaviour
{
    public TMP_Text playerText;
    public TMP_Text gunText;
    public TMP_Text modifiersText;
    public TMP_Text mobsKilledText;
    public TMP_Text timeRemainingText;
    public TMP_Text exitText;

    public TMP_Text levelSummaryLabelText;
    public TMP_Text resultLabelText;
    public TMP_Text characterLabelText;
    public TMP_Text gunLabelText;
    public TMP_Text mobCountLabelText;
    public TMP_Text timeRemainingLabelText;
    public TMP_Text nextStageLabelText;
    public List<TMP_Text> modifierPlaceholders;
    public GameManager gameManager;
    public LocalizationManager localizationManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        localizationManager = LocalizationManager.Instance;

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
        timeRemainingLabelText.text = Mathf.RoundToInt(gameManager.timeRemaining).ToString();
        nextStageLabelText.text = gameManager.levelWon == 1 ? "Market" : "Retry";
        Translate();
        DisplayActiveModifiers();
    }

    void Translate()
    {
        playerText.text = localizationManager.GetLocalizedValue("player");
        gunText.text = localizationManager.GetLocalizedValue("gun");
        modifiersText.text = localizationManager.GetLocalizedValue("modifiers");
        mobsKilledText.text = localizationManager.GetLocalizedValue("mobs_killed");
        timeRemainingText.text = localizationManager.GetLocalizedValue("time_remaining");
        exitText.text = localizationManager.GetLocalizedValue("menu_exit");
        
        levelSummaryLabelText.text = localizationManager.GetLocalizedValue("level_summary") + ":" + gameManager.level.ToString();
        resultLabelText.text = gameManager.levelWon == 1 ? localizationManager.GetLocalizedValue("victory") : localizationManager.GetLocalizedValue("defeat");
        nextStageLabelText.text = gameManager.levelWon == 1 ? localizationManager.GetLocalizedValue("market") : localizationManager.GetLocalizedValue("retry");
    }

    private void DisplayActiveModifiers()
    {
        var activeModifiers = new List<string>();

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
            return null;
        }

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

        for (int i = 0; i < modifierPlaceholders.Count; i++)
        {
            if (i < activeModifiers.Count)
            {
                modifierPlaceholders[i].text = activeModifiers[i];
                modifierPlaceholders[i].gameObject.SetActive(true);
            }
            else
            {
                modifierPlaceholders[i].gameObject.SetActive(false);
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Advance()
    {
        if (gameManager.levelWon == 1)
        {
            SceneManager.LoadScene("Market Menu");
        }
        else
        {
            // destroy singletons
            Destroy(GameManager.Instance.gameObject);
            Destroy(PlayerLevelManager.Instance.gameObject);
            Destroy(LocalizationManager.Instance.gameObject);
            Destroy(MarketConfig.Instance.gameObject);
            SceneManager.LoadScene("Main Menu");
        }
    }
}
