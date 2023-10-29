using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyGroup
{
    /// <summary>
    /// The time in seconds after the wave starts that this group will spawn
    /// </summary>
    public float waveSpawnTime;

    public List<Enemy> enemies;
    public float spawnInterval = 0.25f;
}
