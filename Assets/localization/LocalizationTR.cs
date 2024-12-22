using System.Collections.Generic;
using UnityEngine;

public class LocalizationTR : ILocalization
{
    private readonly Dictionary<string, string> localizedText = new Dictionary<string, string>
    {
        { "menu_play", "Oyna" },
        { "menu_options", "Seçenekler" },
        { "menu_exit", "Çıkış" },
        { "menu_change_language", "Dili Değiştir" },
        { "menu_resume", "Devam Et" },
        { "menu_restart", "Yeniden Başlat" },
        { "menu_settings", "Ayarlar" },
        { "menu_back", "Geri" },
        { "menu_pause", "Duraklat" },
        { "menu_main_menu", "Ana Menü" },
        { "menu_yes", "Evet" },
        { "menu_no", "Hayır" },
        { "assault_rifle_name", "Saldırı Tüfeği" },
        { "assault_rifle_description", "Tüm savaş alanları için çok yönlü bir tüfek." },
        { "shotgun_name", "Pompalı Tüfek" },
        { "shotgun_description", "Yakın mesafede ağır hasar için güçlü." },
        { "grenade_launcher_name", "El Bombası Atar" },
        { "grenade_launcher_description", "Düşman gruplarını vurmak için patlayıcı fırlatır." },
        { "sniper_name", "Keskin Nişancı" },
        { "sniper_description", "Delici mermiler atan güçlü bir silah." },
        { "health_skill_name", "Üstün Sağlık" },
        { "health_skill_description", "Maksimum sağlığınızı artırır." },
        { "speed-skill_name", "Işık Hızı" },
        { "speed-skill_description", "Daha hızlı hareket etmenizi sağlar." },
        { "damage-skill_name", "Ölümcül Hasar" },
        { "damage-skill_description", "Saldırı hasarınızı artırır." },
        { "dodge-skill_name", "Kaçınma Yeteneği" },
        { "dodge-skill_description", "Saldırılardan kaçma şansınızı artırır." },
        { "firerate-skill_name", "Atış Hızı Yeteneği" },
        { "firerate-skill_description", "Silahınızın atış hızını artırır." }
    };

    public string GetValue(string key)
    {
        if (localizedText.ContainsKey(key))
            return localizedText[key];

        Debug.LogWarning($"Key not found: {key}");
        return key;
    }
}
