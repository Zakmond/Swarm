using System.Collections.Generic;
using UnityEngine;

public class LocalizationEN : ILocalization
{
    private readonly Dictionary<string, string> localizedText = new Dictionary<string, string>
    {
        { "menu_play", "Play" },
        { "menu_options", "Options" },
        { "menu_exit", "Exit" },
        { "menu_change_language", "Change Language" },
        { "menu_resume", "Resume" },
        { "menu_restart", "Restart" },
        { "menu_settings", "Settings" },
        { "menu_back", "Back" },
        { "menu_pause", "Pause" },
        { "menu_main_menu", "Main Menu" },
        { "menu_yes", "Yes" },
        { "menu_no", "No" },
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
        { "firerate-skill_description", "Speeds up your weapon's firing rate." }
    };

    public string GetValue(string key)
    {
        if (localizedText.ContainsKey(key))
            return localizedText[key];

        Debug.LogWarning($"Key not found: {key}");
        return key;
    }
}
