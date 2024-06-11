using System;
using UnityEngine;

[Serializable]
public class SubWave
{
    public GameObject enemy;
    public int amount;
    public float rate = 1;
    public float afterwardsAwaitTime;
    public EnemyPath enemyPath = null;

}
