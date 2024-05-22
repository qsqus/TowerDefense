using System;
using UnityEngine;

[Serializable]
public class SubWave
{
    public GameObject enemy;
    public int amount;
    public float rate;
    public float afterwardsAwaitTime;
    public EnemyPath enemyPath = null;

}
