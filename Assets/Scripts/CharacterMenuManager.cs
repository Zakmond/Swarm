using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMenuManager : MonoBehaviour
{
    public void SelectCharacter(string characterName)
    {
        PlayerLevelManager.Instance.UpdateCharacter(characterName);

        SceneManager.LoadScene("Level 1");
    }
}
