using TMPro;
using UnityEngine;

public class LevelStatsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text coins;
    [SerializeField] private TMP_Text diamods;
    [SerializeField] private TMP_Text lives;


    private void Start()
    {
        LevelManager.instance.OnCoinsAmountChanged += LevelManager_OnCoinsAmountChanged;
        LevelManager.instance.OnDiamondsAmountChanged += LevelManager_OnDiamondsAmountChanged;
        LevelManager.instance.OnLivesAmountChanged += LevelManager_OnLivesAmountChanged;
    }

    private void LevelManager_OnLivesAmountChanged(int amount)
    {
        lives.text = amount.ToString();
    }

    private void LevelManager_OnDiamondsAmountChanged(int amount)
    {
        diamods.text = amount.ToString();
    }

    private void LevelManager_OnCoinsAmountChanged(int amount)
    {
        coins.text = amount.ToString();
    }
}
