using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private string levelDisplayedNumber = "1";
    [SerializeField] private int levelBuildNumber = 0;
    [SerializeField] private TMP_Text levelName;
    [SerializeField] private bool destroyMusicManager = true;

    private void Awake()
    {
        levelName.text = levelName.text + levelDisplayedNumber;
    }

    public void OnLevelSelected()
    {
        if(destroyMusicManager)
        {
            MusicManager.instance.DestroyMusicManager();
        }

        SceneManager.LoadScene(levelBuildNumber);
    }

}
