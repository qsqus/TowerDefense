using TMPro;
using UnityEngine;

public class LevelStatsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text coins;
    [SerializeField] private TMP_Text diamods;
    [SerializeField] private TMP_Text lives;
    [SerializeField] private TMP_Text waveProgress;
    [SerializeField] private Animator waveProgressAnimator;

    // Execution order: Awake, OnEnable, Start
    private void OnEnable()
    {
        LevelManager.instance.OnCoinsAmountChanged += LevelManager_OnCoinsAmountChanged;
        LevelManager.instance.OnDiamondsAmountChanged += LevelManager_OnDiamondsAmountChanged;
        LevelManager.instance.OnLivesAmountChanged += LevelManager_OnLivesAmountChanged;
        LevelManager.instance.OnNewWaveStarted += LevelManager_OnNewWaveStarted;
    }

    private void LevelManager_OnNewWaveStarted(int currentWave, int totalWaves)
    {
        waveProgress.text = $"Wave {currentWave} {totalWaves}";
        waveProgressAnimator.SetTrigger("NewWaveTrigger");
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
