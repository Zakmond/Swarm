using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMenuManager : MonoBehaviour
{
    public LocalizationManager localizationManager;
    public TMP_Text noxText;
    public TMP_Text aresText;
    public TMP_Text characterText;
    void Start()
    {
        localizationManager = LocalizationManager.Instance;
        noxText.text = localizationManager.GetLocalizedValue("nox_summary");
        aresText.text = localizationManager.GetLocalizedValue("ares_summary");
        characterText.text = localizationManager.GetLocalizedValue("character");
    }
    public void SelectCharacter(string characterName)
    {
        PlayerLevelManager.Instance.UpdateCharacter(characterName);

        SceneManager.LoadScene("level1");
    }
}
