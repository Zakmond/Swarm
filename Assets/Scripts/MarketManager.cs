using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class MarketManager : MonoBehaviour
{

    private MarketConfig marketConfig; 
    private PlayerLevelManager playerLevelManager;
    private GameManager gameManager;
    public LocalizationManager localizationManager;
    private string itemKey;
    public TMP_Text coinText;
    public TMP_Text continueText;
    void Start()
    {
        localizationManager = LocalizationManager.Instance;
        marketConfig = MarketConfig.Instance;
        playerLevelManager = PlayerLevelManager.Instance;
        gameManager = GameManager.Instance;
        continueText.text = localizationManager.GetLocalizedValue("continue");
        coinText.text = playerLevelManager.GetCurrency().ToString();
    }

    void Update()
    {

    }

    public void SetItemKey(string key)
    {
        itemKey = key;
    }
    public void BuyItem()
    {
        if (!marketConfig.Items.TryGetValue(itemKey, out var itemData))
        {
            Debug.LogError($"Item key {itemKey} not found in market config.");
            return;
        }
        int price = itemData.price;
        int currency = playerLevelManager.GetCurrency();

        if (currency < price)
        {
            return;
        }
        if (itemData.type == "skill")
        {
            string ability = itemData.ability;
            if (float.TryParse(itemData.ability_effect.ToString(), out float effectValue))
            {
                ApplySkill(ability, effectValue);
            }
        }
        else if (itemData.type == "weapon")
        {
            ApplyWeapon(itemData.name);
        }
        playerLevelManager.SubTrackCurrency(itemData.price);

        coinText.text = playerLevelManager.GetCurrency().ToString();
    }
private void ApplyWeapon(string weaponName)
    {
        switch (weaponName)
        {
            case "assault_rifle_name":
                playerLevelManager.playerData.weapon = "RifleHolder";
                break;
            case "shotgun_name":
                playerLevelManager.playerData.weapon = "ShotgunHolder";
                break;
            case "grenade_launcher_name":
                playerLevelManager.playerData.weapon = "GrenadelauncherHolder";
                break;
            case "sniper_name":
                playerLevelManager.playerData.weapon = "SniperHolder";
                break;
            default:
                Debug.LogWarning($"Unknown weapon: {weaponName}");
                break;
        }
    }
private void ApplySkill(string ability, float effectValue)
    {
        switch (ability)
        {
            case "max_health":
                playerLevelManager.UpdateHealthModifier(effectValue);
                break;
            case "speed":
                playerLevelManager.UpdateSpeedModifier(effectValue);
                break;
            case "damage":
                playerLevelManager.UpdateDamageModifier(effectValue);
                break;
            case "fire_rate":
                playerLevelManager.UpdateFireRateModifier(effectValue);
                break;
            case "dodge":
                playerLevelManager.UpdateDodgeChanceModifier(effectValue);
                break;
            case "max_ammo":
                playerLevelManager.UpdateMaxAmmoModifier(effectValue);
                break;
            default:
                Debug.LogWarning($"Unknown ability: {ability}");
                break;
        }
    }

    public void NextLevel()
    {
        Debug.Log("Next Level");
        Debug.Log($"level{gameManager.level + 1}");
        SceneManager.LoadScene($"level{gameManager.level + 1}");
    }

}
