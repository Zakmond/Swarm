using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private ILocalization currentLocalization;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetLanguage("en"); 
            DontDestroyOnLoad(gameObject);
            // test language
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
                break;
            case "tr":
                currentLocalization = new LocalizationTR();
                break;
            default:
                Debug.LogWarning($"Unsupported language code: {languageCode}");
                currentLocalization = new LocalizationEN(); // Fallback
                break;
        }
    }

    public string GetLocalizedValue(string key)
    {
        return currentLocalization?.GetValue(key) ?? key;
    }
}
