using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
