using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private float timeBetweenWaves = 5f;

    public static int EnemiesAlive = 0;

    private float countdown = 2f;
    private int waveIndex = 0;
    private int amountOfWaves;

    private void Start()
    {
        amountOfWaves = waves.Length;
    }

    void Update()
    {
        // Prevents starting new wave when enemies are still alive
        if(EnemiesAlive > 0)
        {
            return;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;

    }

    // Spawns wave
    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.subWaves.Length; i++)
        {
            SubWave subWave = wave.subWaves[i];

            EnemiesAlive += subWave.amount;

            for (int j = 0; j < subWave.amount; j++)
            {
                SpawnEnemy(subWave.enemy);
                yield return new WaitForSeconds(1f / subWave.rate);
            }

            if (subWave.afterwardsAwaitTime != 0 && i != wave.subWaves.Length - 1)
            {
                yield return new WaitForSeconds(subWave.afterwardsAwaitTime);
            }
        }

        waveIndex += 1;

        if (waveIndex == amountOfWaves)
        {
            Debug.Log("Level finished");
            
            // Disables this script
            this.enabled = false;
        }
    }


    // Spawns enemy as its child
    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, transform.position, transform.rotation, transform);
    }


}
