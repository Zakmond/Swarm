using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class WeaponHoverInfo : MonoBehaviour
{
    public GameObject infoPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public TextMeshProUGUI itemDescriptionText;
    public UnityEngine.UI.Image itemImage;
    public string itemKey; 

    private static bool isPanelLocked = false;
    private static WeaponHoverInfo lockedItem = null;

    public MarketConfig marketConfig;
    [SerializeField] private LocalizationManager localizationManager;
    [SerializeField] private PlayerLevelManager playerLevelManager;
    public MarketManager marketManager;
    private void Start()
    {
        marketConfig = MarketConfig.Instance;
        localizationManager = LocalizationManager.Instance;
        playerLevelManager = PlayerLevelManager.Instance;

        if (marketConfig == null || localizationManager == null || playerLevelManager == null)
        {
            Debug.LogError("MarketConfig, LocalizationManager, or PlayerLevelManager is not properly initialized.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPanelLocked)
        {
            ShowInfo();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPanelLocked)
        {
            HideInfo();
        }
    }

    public void OnButtonClick()
    {
        if (isPanelLocked && lockedItem == this)
        {
            UnlockPanel();
        }
        else
        {
            LockPanel();
        }
    }

    private void LockPanel()
    {
        isPanelLocked = true;
        lockedItem = this;
        ShowInfo();
    }

    private void UnlockPanel()
    {
        isPanelLocked = false;
        lockedItem = null;
        HideInfo();
    }

    private void ShowInfo()
    {
        Debug.Log(itemKey);
        Debug.Log(marketConfig.Items);
        MarketItem itemData = marketConfig.Items[itemKey];

        string itemName = localizationManager.GetLocalizedValue(itemData.name);
        string itemDescription = localizationManager.GetLocalizedValue(itemData.description);
        string itemPrice = itemData.price.ToString();

        itemNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemPriceText.text = itemPrice;
        Sprite currentIcon = GetComponent<Image>().sprite;
        itemImage.sprite = currentIcon;
        marketManager.SetItemKey(itemKey);
        infoPanel.SetActive(true);
    }

    private void HideInfo()
    {
        infoPanel.SetActive(false);
    }

    public void BuyItem()
    {
        if (!marketConfig.Items.TryGetValue(itemKey, out var itemData))
        {
            Debug.LogError($"Item key {itemKey} not found in market config.");
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
        playerLevelManager.SubTrackCurrency(itemData.price);
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
            case "dodge_chance":
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
}
