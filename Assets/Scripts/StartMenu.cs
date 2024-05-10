using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void OnPlayButtonPressed()
    {
        Debug.Log("Play");
        SceneManager.LoadScene(1);
    }

    public void OnQuitButtonPressed()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OnSettingsButtonPressed()
    {
        Debug.Log("Settings");
        Application.Quit();
    }

}
