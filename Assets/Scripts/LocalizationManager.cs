using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private ILocalization currentLocalization;
    public event Action<bool> languageChanged;
    public string currentLanguage = "en";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetLanguage(currentLanguage); 
            Debug.Log("Localization Manager started.");
            Debug.Log("Current language: " + GetLocalizedValue("menu_start"));
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetLanguage(string languageCode)
    {
        switch (languageCode)
        {
            case "en":
                currentLocalization = new LocalizationEN();
                currentLanguage = "en";
                break;
            case "tr":
                currentLocalization = new LocalizationTR();
                currentLanguage = "tr";
                break;
            default:
                Debug.LogWarning($"Unsupported language code: {languageCode}");
                currentLocalization = new LocalizationEN(); // Fallback
                break;
        }
        languageChanged?.Invoke(true);
    }

    public string GetLocalizedValue(string key)
    {
        return currentLocalization?.GetValue(key) ?? key;
    }
}
