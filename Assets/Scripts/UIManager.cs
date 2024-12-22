using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RectTransform healthFill;
    private PlayerController playerController;
    private PlayerLevelManager playerLevelManager;
    private NPCLevelManager npcLevelManager;
    private WeaponBase currentWeapon;
    public TMP_Text timerText;
    public TMP_Text moneyTextObj;
    private Animator reloadAnimator;
    public GameObject reloadObj;
    public TMP_Text ammoText;
    public GameObject dodgeEffectObj;

    void Start()
    {
        // Find PlayerController in the scene
        playerController = FindObjectOfType<PlayerController>();
        playerLevelManager = FindObjectOfType<PlayerLevelManager>();
        npcLevelManager = FindObjectOfType<NPCLevelManager>();
        moneyTextObj.text = playerLevelManager.GetCurrency().ToString();
        if (npcLevelManager != null)
        {
            npcLevelManager.OnKill += UpdateCurrency;
        }
        if (playerController != null)
        {
            playerController.OnHealthChanged += UpdateHealthBar;
            playerController.OnDodge += HandleDodge;
            UpdateHealthBar(playerController.GetHealthPercentage());

            currentWeapon = playerController.GetComponentInChildren<WeaponBase>();
            if (currentWeapon != null)
            {
                currentWeapon.OnReloadStarted += HandleReloadStarted;
                currentWeapon.OnAmmoChange += updateAmmoText;
            }

            updateAmmoText(currentWeapon.GetAmmo().Item1, currentWeapon.GetAmmo().Item2);
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }

        // Cache the reloadObj animator
        if (reloadObj != null)
        {
            reloadAnimator = reloadObj.GetComponent<Animator>();
        }
    }
    private void HandleReloadStarted(float reloadTime)
    {
        if (reloadAnimator != null && reloadObj != null)
        {
            reloadObj.SetActive(true);

            reloadAnimator.SetFloat("speed", 1f / reloadTime);

            StartCoroutine(DeactivateReloadObjAfterDelay(reloadTime));
        }
    }

    private System.Collections.IEnumerator DeactivateReloadObjAfterDelay(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);

        reloadObj.SetActive(false);
    }

    void Update()
    {
        if (NPCLevelManager.Instance != null)
        {
            timerText.text = NPCLevelManager.Instance.levelTimer.ToString("F0");
        }
    }
    void UpdateHealthBar(float healthPercent)
    {
        // Change the width of the health fill based on the health percentage
        float newWidth = Math.Max(0, healthPercent * 245);
        healthFill.sizeDelta = new Vector2(newWidth, healthFill.sizeDelta.y);
    }

    void HandleDodge(bool dodged)
    {
        if (dodgeEffectObj != null)
        {
            dodgeEffectObj.SetActive(true);
            dodgeEffectObj.GetComponent<Animator>().SetTrigger("Dodge");
            StartCoroutine(DeactivateDodgeEffect(0.3f));

        }

    }

    private System.Collections.IEnumerator DeactivateDodgeEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        dodgeEffectObj.SetActive(false);
    }
    void updateAmmoText(int currentAmmo, int maxAmmo)
    {
        ammoText.text = currentAmmo + "/" + maxAmmo;
    }
    void UpdateCurrency(int currency)
    {
        playerLevelManager.UpdateCurrency(currency);
        moneyTextObj.text = playerLevelManager.GetCurrency().ToString();

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
