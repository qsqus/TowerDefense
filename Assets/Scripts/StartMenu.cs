using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private ButtonTextVisual settingsButton;

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
        settings.ToggleSettingsVisible(true);
        settingsButton.OnPointerExit();
        Debug.Log("Settings");
    }

}
