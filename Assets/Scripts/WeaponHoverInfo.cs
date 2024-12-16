using UnityEngine;
using TMPro; 
using UnityEngine.EventSystems; 

public class WeaponHoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoPanel;             
    public TextMeshProUGUI weaponNameText;   
    public TextMeshProUGUI weaponPriceText;  
    public TextMeshProUGUI weaponDescriptionText; 
    public UnityEngine.UI.Image weaponImage; 

    public string weaponName;   
    public string weaponPrice;  
    public string weaponDescription; 
    public Sprite weaponSprite; 

    private static bool isPanelLocked = false; 
    private static WeaponHoverInfo lockedWeapon = null; 

   
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
        if (isPanelLocked && lockedWeapon == this) 
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
        lockedWeapon = this; 
        ShowInfo(); 
    }

    
    private void UnlockPanel()
    {
        isPanelLocked = false; 
        lockedWeapon = null; 
        HideInfo(); 
    }

    
    private void ShowInfo()
    {
        infoPanel.SetActive(true);
        weaponNameText.text = weaponName; 
        weaponPriceText.text = weaponPrice; 
        weaponDescriptionText.text = weaponDescription; 
        weaponImage.sprite = weaponSprite; 
    }

    
    private void HideInfo()
    {
        infoPanel.SetActive(false);
    }
}
