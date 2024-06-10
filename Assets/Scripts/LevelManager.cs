using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int startLivesAmount = 20;
    [SerializeField] private int coinsAmount = 400;
    [SerializeField] private int diamondsAmount = 0;

    [Header("Collectible types")]
    public GameObject coin;
    public GameObject diamond;

    public event Action<int> OnCoinsAmountChanged;
    public event Action<int> OnDiamondsAmountChanged;
    public event Action<int> OnLivesAmountChanged;
    public event Action<int, int> OnNewWaveStarted;

    public static LevelManager instance { get; private set; }

    private int coinWorth;
    private int currentLivesAmount;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of LevelManager in scene");

            Destroy(gameObject);
            return;
        }
        instance = this;
        currentLivesAmount = startLivesAmount;
    }

    void Start()
    {
        coinWorth = coin.GetComponent<Collectible>().GetWorth();

        OnCoinsAmountChanged?.Invoke(coinsAmount);
        OnDiamondsAmountChanged?.Invoke(diamondsAmount);
        OnLivesAmountChanged?.Invoke(currentLivesAmount);
    }
    
    public void ChangeCoinsByAmount(int amount)
    {
        coinsAmount += amount;
        OnCoinsAmountChanged?.Invoke(coinsAmount);

    }

    public int GetCoinsAmount()
    {
        return coinsAmount;
    }

    public void ChangeDiamondsByAmount(int amount)
    {
        diamondsAmount += amount;
        OnDiamondsAmountChanged?.Invoke(diamondsAmount);

    }

    public int GetDiamondsAmount()
    {
        return diamondsAmount;
    }

    public int GetCoinWorth()
    {
        return coinWorth;
    }

    public void ChangeLivesByAmount(int amount)
    {
        currentLivesAmount += amount;
        OnLivesAmountChanged?.Invoke(currentLivesAmount);

        if(currentLivesAmount <= 0)
        {
            Debug.Log("Game over");
            ShowLevelFinishedScreen("You lose");
        }

    }

    public void ShowLevelFinishedScreen(string result)
    {
        TowerManager.instance.AttemptHideTowerBuildMenu();
        LevelFinished.instance.InitializeLevelFinished(result, currentLivesAmount, startLivesAmount);
    }


    public void ShowWaveProgress(int currentWave, int totalWaves)
    {
        Debug.Log($"Wave {currentWave}/{totalWaves}");
        OnNewWaveStarted?.Invoke(currentWave, totalWaves);
    }

}
