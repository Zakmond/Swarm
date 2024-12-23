using System.Collections.Generic;
using UnityEngine;

public class LocalizationTR : ILocalization
{
    private readonly Dictionary<string, string> localizedText = new Dictionary<string, string>
    {
        { "menu_start", "Başlat" },
        { "menu_settings", "Ayarlar" },
        { "menu_exit", "Çıkış" },
        { "menu_languages", "Dil" },
        { "menu_resume", "Devam Et" },
        { "menu_restart", "Yeniden Başlat" },
        { "menu_music", "Müzik" },
        { "menu_support", "Destek" },
        { "menu_audio", "Ses" },
        { "assault_rifle_name", "Hücum Tüfeği" },
        { "assault_rifle_description", "Her savaş mesafesi için çok yönlü bir tüfek." },
        { "shotgun_name", "Pompalı Tüfek" },
        { "shotgun_description", "Yakın mesafede yüksek hasar için güçlü bir silah." },
        { "grenade_launcher_name", "El Bombası Fırlatıcı" },
        { "grenade_launcher_description", "Düşman gruplarını vurmak için patlayıcılar fırlatır." },
        { "sniper_name", "Keskin Nişancı Tüfeği" },
        { "sniper_description", "Delici mermiler atan güçlü bir silah." },
        { "health_skill_name", "Üstün Sağlık" },
        { "health_skill_description", "Maksimum sağlığınızı artırır." },
        { "speed-skill_name", "Işık Hızı" },
        { "speed-skill_description", "Daha hızlı hareket etmenizi sağlar." },
        { "damage-skill_name", "Ölümcül Hasar" },
        { "damage-skill_description", "Saldırı gücünüzü artırır." },
        { "dodge-skill_name", "Kaçınma Becerisi" },
        { "dodge-skill_description", "Saldırılardan kaçınma şansınızı artırır." },
        { "firerate-skill_name", "Atış Hızı Becerisi" },
        { "firerate-skill_description", "Silahınızın ateş hızını artırır." },
        {"maxammo-skill_name", "Maksimum Cephane Becerisi"},
        {"maxammo-skill_description", "Maksimum cephane kapasitenizi artırır."},
        {"continue", "Devam"},
        {"level_summary", "Seviye Özeti"},
        {"victory", "Zafer!"},
        {"defeat", "Yenilgi!"},
        {"character", "Karakter"},
        {"gun", "Silah"},
        {"mobs_killed", "Öldürülen Düşmanlar"},
        {"time_remaining", "Kalan Süre"},
        {"market", "Pazar"},
        {"retry", "Tekrar Dene"},
        {"nox_summary", "Hızlı hareket eden, hafif zırhlı bir kadın nişancı. Daha yüksek hız ve çeviklik, daha hızlı atış hızı ama daha düşük savunma sunar."},
        {"ares_summary", "Ağır zırhlı bir erkek savaşçı. Daha düşük hız ama daha fazla sağlık. Daha düşük atış hızı ama daha yüksek savunma sunar."},
        {"modifiers", "Değiştiriciler"},
        {"player", "Oyuncu"}
    };

    public string GetValue(string key)
    {
        if (localizedText.ContainsKey(key))
            return localizedText[key];

        Debug.LogWarning($"Anahtar bulunamadı: {key}");
        return key;
    }
}
