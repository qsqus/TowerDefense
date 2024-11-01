using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private EnemyPath defaultEnemyPath;

    public static int EnemiesAlive;

    private float countdown = 2f;
    private int waveIndex = 0;
    private int amountOfWaves;

    private void Awake()
    {
        EnemiesAlive = 0;
    }

    private void Start()
    {
        amountOfWaves = waves.Length;
    }

    void Update()
    {
        // Prevents starting new wave when enemies are still alive
        if (EnemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == amountOfWaves)
        {
            Debug.Log("Level finished");
            LevelManager.instance.ShowLevelFinishedScreen("You win");

            // Disables this script
            this.enabled = false;
            return;
        }

        if (countdown <= 0f)
        {
            LevelManager.instance.ShowWaveProgress(waveIndex+1, amountOfWaves);

            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;

    }

    // Spawns wave
    private IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.subWaves.Length; i++)
        {
            SubWave subWave = wave.subWaves[i];

            EnemiesAlive += subWave.amount;

            for (int j = 0; j < subWave.amount; j++)
            {
                EnemyPath spawnPath = subWave.enemyPath;

                if(spawnPath != null)
                {
                    SpawnEnemy(subWave.enemy, spawnPath);
                }
                else
                {
                    SpawnEnemy(subWave.enemy, defaultEnemyPath);
                }
                float spawnRate = subWave.rate > 0 ? subWave.rate : 0.5f;

                yield return new WaitForSeconds(1f / spawnRate);
            }

            if (subWave.afterwardsAwaitTime != 0 && i != wave.subWaves.Length - 1)
            {
                yield return new WaitForSeconds(subWave.afterwardsAwaitTime);
            }
        }

        waveIndex += 1;

        if (waveIndex == amountOfWaves)
        {
            Debug.Log("All waves finished");
        }
    }


    // Spawns enemy as its child on a path
    private void SpawnEnemy(GameObject enemy, EnemyPath enemyPath)
    {
        Enemy enemyInstance = Instantiate(enemy, enemyPath.GetEnemyPathElement(0).position, transform.rotation, transform).GetComponent<Enemy>();
        enemyInstance.SetEnemyPath(enemyPath);
    }


}
