using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{
    private const string MainMenuSceneName = "Main_menu";

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        FileHandler.Reset(RaceCompletion.racesFilename);
        RaceCompletion.ResetCompletionData();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
