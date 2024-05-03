using System.Runtime.InteropServices;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Collectible types")]
    public GameObject coin;
    public GameObject diamond;

    public static LevelManager instance { get; private set; }

    private int coinsAmount = 0;
    private int diamondsAmount = 0;
    private int livesAmount;
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
    }

    void Update()
    {
        
    }

    public void ChangeCoinsByAmount(int amount)
    {
        coinsAmount += amount;
        Debug.Log(coinsAmount);
    }

    public int GetCoinsAmount()
    {
        return coinsAmount;
    }

    public void ChangeDiamondsByAmount(int amount)
    {
        diamondsAmount += amount;
    }

    public int GetDiamondsAmount()
    {
        return diamondsAmount;
    }

    public int GetCoinWorth()
    {
        return coinWorth;
    }

}
