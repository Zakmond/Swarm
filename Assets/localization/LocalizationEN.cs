using System.Collections.Generic;
using UnityEngine;

public class LocalizationEN : ILocalization
{
    private readonly Dictionary<string, string> localizedText = new Dictionary<string, string>
    {
        { "menu_start", "Start" },
        { "menu_settings", "Settings" },
        { "menu_exit", "Exit" },
        { "menu_languages", "Language" },
        { "menu_resume", "Resume" },
        { "menu_restart", "Restart" },
        { "menu_music", "Music"},
        { "menu_support", "Support"},
        { "menu_audio", "Audio"},
        { "assault_rifle_name", "Assault Rifle" },
        { "assault_rifle_description", "A versatile rifle for all combat ranges." },
        { "shotgun_name", "Shotgun" },
        { "shotgun_description", "Powerful at close range for heavy damage." },
        { "grenade_launcher_name", "Grenade Launcher" },
        { "grenade_launcher_description", "Fires explosives to hit groups of enemies." },
        { "sniper_name", "Sniper" },
        { "sniper_description", "Powerful weapon that shoots piercing bullets." },
        { "health_skill_name", "Supreme Health" },
        { "health_skill_description", "Increases your maximum health." },
        { "speed-skill_name", "Speed Of Light" },
        { "speed-skill_description", "Makes you move faster." },
        { "damage-skill_name", "Fatal Damage" },
        { "damage-skill_description", "Boosts your attack damage." },
        { "dodge-skill_name", "Dodge Skill" },
        { "dodge-skill_description", "Improves your chance to dodge attacks." },
        { "firerate-skill_name", "Firerate Skill" },
        { "firerate-skill_description", "Speeds up your weapon's firing rate." },
        {"maxammo-skill_name", "Max Ammo Skill"},
        {"maxammo-skill_description", "Increases your maximum ammo capacity."},
        {"continue", "continue"},
        {"level_summary", "Level Summary"},
        {"victory", "Victory!"},
        {"defeat", "Defeat!"},
        {"character", "Character"},
        {"gun", "Gun"},
        {"mobs_killed", "Mobs Killed"},
        {"time_remaining", "Time Remaining"},
        {"market", "Market"},
        {"retry", "Retry"},
        {"nox_summary", "A fast-moving, lightly armored female marksman. Offers higher speed and agility, faster fire rate but lower defense."},
        {"ares_summary", "A heavy-armored male warrior. Has lower speed but more health. Lower fire rate but higher defense."},
        {"modifiers", "Modifiers"},
        {"player", "Player"},
        {"RifleHolder", "Assault Rifle"},
        {"ShotgunHolder", "Shotgun"},
        {"GrenadelauncherHolder", "Grenade Launcher"},
        {"SniperHolder", "Sniper"},
    };

    public string GetValue(string key)
    {
        if (localizedText.ContainsKey(key))
            return localizedText[key];

        Debug.LogWarning($"Key not found: {key}");
        return key;
    }
}
