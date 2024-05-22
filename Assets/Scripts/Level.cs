using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private string levelDisplayedNumber = "1";
    [SerializeField] private int levelBuildNumber = 0;
    [SerializeField] TMP_Text levelName;

    private void Awake()
    {
        levelName.text = levelName.text + levelDisplayedNumber;
    }

    public void OnLevelSelected()
    {
        SceneManager.LoadScene(levelBuildNumber);
    }

}
