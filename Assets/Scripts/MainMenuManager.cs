using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text startText;
    public TMP_Text settingsText;
    public TMP_Text quitText;
    public TMP_Text languagesText;
    public TMP_Text musicButtonText;
    public TMP_Text supportText;
    public TMP_Text audioText;
    public TMP_Text musicText;
    public LocalizationManager localizationManager;
    void Start()
    {
        SetText(true);
        localizationManager.languageChanged += SetText;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Character Selection");

    }


    private void SetText(bool t)
    {
        startText.text = localizationManager.GetLocalizedValue("menu_start");
        settingsText.text = localizationManager.GetLocalizedValue("menu_settings");
        quitText.text = localizationManager.GetLocalizedValue("menu_exit");
        languagesText.text = localizationManager.GetLocalizedValue("menu_languages");
        musicButtonText.text = localizationManager.GetLocalizedValue("menu_music");
        musicText.text = localizationManager.GetLocalizedValue("menu_music");
        supportText.text = localizationManager.GetLocalizedValue("menu_support");
        audioText.text = localizationManager.GetLocalizedValue("menu_audio");
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
