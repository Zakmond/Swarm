using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class MarketConfigWrapper
{
    public List<MarketItemEntry> items;
}

[System.Serializable]
public class MarketItemEntry
{
    public string key;
    public MarketItem value;
}
[System.Serializable]
public class MarketItem
{
    public string type;
    public int price;
    public string name;
    public string description;
    public string ability;
    public float ability_effect;

}

public class MarketConfig : MonoBehaviour
{
    public static MarketConfig Instance;

    public Dictionary<string, MarketItem> Items { get; private set; }
    public TextAsset ItemsFile;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMarketConfig();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadMarketConfig()
{
    if (ItemsFile == null)
    {
        Debug.LogError("ItemsFile not assigned in the inspector!");
        return;
    }

    var wrapper = JsonUtility.FromJson<MarketConfigWrapper>(ItemsFile.text);
    if (wrapper != null && wrapper.items != null)
    {
        Items = new Dictionary<string, MarketItem>();
        foreach (var entry in wrapper.items)
        {
            Items[entry.key] = entry.value;
        }
    }
    else
    {
        Debug.LogError("Failed to deserialize MarketConfig.");
    }
}

}