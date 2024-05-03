using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    private int coinsAmount = 0;
    private int diamondsAmount = 0;
    private int livesAmount;

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
        
    }

    void Update()
    {
        
    }

    public void ChangeCoinsAmount(int amount)
    {
        coinsAmount += amount;
        Debug.Log(coinsAmount);
    }

    public int GetCoinsAmount()
    {
        return coinsAmount;
    }

    public void ChangeDiamondsAmount(int amount)
    {
        diamondsAmount += amount;
    }

    public int GetDiamondsAmount()
    {
        return diamondsAmount;
    }



}
