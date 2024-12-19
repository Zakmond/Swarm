using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransitionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text levelSummaryLabelText;
    public TMP_Text resultLabelText;
    public TMP_Text characterLabelText;
    public TMP_Text gunLabelText;
    public TMP_Text mobCountLabelText;
    public TMP_Text modifiersLabelText;
    public TMP_Text timeRemainingLabelText;
    public TMP_Text nextStageLabelText;

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
        modifiersLabelText.text = "Max Health: " + gameManager.maxHealthModifier + "\n" +
            "Speed: " + gameManager.speedModifier + "\n" +
            "Damage: " + gameManager.damageModifier + "\n" +
            "Fire Rate: " + gameManager.fireRateModifier + "\n" +
            "Dodge Chance: " + gameManager.dodgeChanceModifier + "\n" +
            "Max Ammo: " + gameManager.maxAmmoModifier;
        timeRemainingLabelText.text = gameManager.timeRemaining.ToString();
        nextStageLabelText.text = gameManager.levelWon == 1 ? "Next Stage" : "Retry";

    }

}
