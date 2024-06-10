using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private ButtonTextVisual[] buttons;
    [SerializeField] private Settings settings;
    [SerializeField] private ButtonTextVisual settingsButton;

    public static bool IsPaused = false;
    public static bool IsLevelOver = false;

    private void Awake()
    {
        IsPaused = false;
        IsLevelOver = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!IsLevelOver)
            {
                if (IsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }

        }

    }

    public void Resume()
    {
        settings.ToggleSettingsVisible(false);

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;

        foreach(ButtonTextVisual button in buttons)
        {
            button.OnPointerExit();
        }
    }

    private void Pause()
    {
        foreach (ButtonTextVisual button in buttons)
        {
            button.SetNormalColor();
        }

        TowerManager.instance.AttemptHideTowerBuildMenu();

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void OnSettingsButtonPressed()
    {
        settings.ToggleSettingsVisible(true);
        settingsButton.OnPointerExit();
        Debug.Log("Settings");
    }


    public void OnRestartButtonPressed()
    {
        Debug.Log("Restart");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Exit");
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

}
