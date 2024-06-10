using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinished : MonoBehaviour
{
    [SerializeField] TMP_Text levelResult;
    [SerializeField] TMP_Text livesResult;
    [SerializeField] GameObject resultsScreen;

    public static LevelFinished instance { get; private set; }

    private bool wasInitialized = false;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of LevelFinished in scene");

            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void InitializeLevelFinished(string result, int lives, int startLives)
    {
        if (!wasInitialized)
        {
            PauseMenu.IsLevelOver = true;
            Time.timeScale = 0f;
            resultsScreen.SetActive(true);

            levelResult.text = result;
            livesResult.text = lives.ToString() + " out of " + startLives.ToString();

            wasInitialized = true;
        }

    }

    public void OnRestartButtonPressed()
    {
        Debug.Log("Try again");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Leave");
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }


}
