using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCharacterSelect()
    {
        SceneManager.LoadScene("Character Select");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
