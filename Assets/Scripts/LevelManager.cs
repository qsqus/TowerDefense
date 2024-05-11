using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int livesAmount = 20;
    [SerializeField] private int coinsAmount = 400;
    [SerializeField] private int diamondsAmount = 0;

    [Header("Collectible types")]
    public GameObject coin;
    public GameObject diamond;

    public event Action<int> OnCoinsAmountChanged;
    public event Action<int> OnDiamondsAmountChanged;
    public event Action<int> OnLivesAmountChanged;

    public static LevelManager instance { get; private set; }

    private int coinWorth;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of LevelManager in scene");

            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        coinWorth = coin.GetComponent<Collectible>().GetWorth();

        OnCoinsAmountChanged?.Invoke(coinsAmount);
        OnDiamondsAmountChanged?.Invoke(diamondsAmount);
        OnLivesAmountChanged?.Invoke(livesAmount);

    }

    public void ChangeCoinsByAmount(int amount)
    {
        coinsAmount += amount;
        OnCoinsAmountChanged?.Invoke(coinsAmount);

        Debug.Log(coinsAmount);
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
        livesAmount += amount;
        OnLivesAmountChanged?.Invoke(livesAmount);

        if(livesAmount <= 0)
        {
            Debug.Log("Game over");
        }

    }

}
