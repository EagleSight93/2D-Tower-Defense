using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<EnemyWave> waves;

    void OnEnable()
    {
        GameEvents.OnWaveStarted += StartWave;
    }
    void OnDisable()
    {
        GameEvents.OnWaveStarted -= StartWave;
    }

    void StartWave(int waveNumber)
    {
        if (waveNumber >= waves.Count) return;

        StartCoroutine(SpawnWave(waves[waveNumber]));
    }

    IEnumerator SpawnWave(EnemyWave wave)
    {
        List<EnemyGroup> enemyGroups = wave.enemyGroups;
        int groupIndex = 0;

        float currentTime = 0f;
        while (currentTime < wave.duration && groupIndex < enemyGroups.Count)
        {
            currentTime += Time.deltaTime;
            yield return null;

            if (currentTime < enemyGroups[groupIndex].waveSpawnTime) continue;

            StartCoroutine(SpawnGroup(enemyGroups[groupIndex]));
            groupIndex++;
        }
    }

    IEnumerator SpawnGroup(EnemyGroup group)
    {
        foreach (Enemy enemy in group.enemies)
        {
            Instantiate(enemy, transform, false);
            yield return new WaitForSeconds(group.spawnInterval);
        }
    }
}
