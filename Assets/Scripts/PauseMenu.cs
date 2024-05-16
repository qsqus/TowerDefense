using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private ButtonTextVisual[] buttons;

    public static bool IsPaused = false;

    private void Awake()
    {
        IsPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }

    public void Resume()
    {
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
        SceneManager.LoadScene(0);
    }

}
