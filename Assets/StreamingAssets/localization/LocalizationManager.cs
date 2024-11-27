using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private Dictionary<string, string> localizedText;
    private string currentLanguage = "en"; // default language

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLanguage(string language)
    {
        currentLanguage = language;
        string filePath = Path.Combine(Application.streamingAssetsPath, $"Localization/{language}.json");

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            localizedText = JsonUtility.FromJson<Dictionary<string, string>>(jsonData);
        }
        else
        {
            Debug.LogError($"Localization file not found: {filePath}");
        }
    }

    public string GetLocalizedValue(string key)
    {
        if (localizedText != null && localizedText.ContainsKey(key))
        {
            return localizedText[key];
        }

        Debug.LogWarning($"Key not found in localization data: {key}");
        return key;
    }

    public string CurrentLanguage => currentLanguage;
}
