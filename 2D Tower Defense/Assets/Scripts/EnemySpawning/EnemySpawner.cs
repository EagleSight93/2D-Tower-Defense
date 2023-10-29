using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Min(1)] [SerializeField] int startWave = 1;
    [SerializeField] List<EnemyWave> waves;

    void OnEnable()
    {
        GameEvents.OnWaveNumberStarted += StartWaveNumber;
    }
    void OnDisable()
    {
        GameEvents.OnWaveNumberStarted -= StartWaveNumber;
    }

    void StartWaveNumber(int waveNumber)
    {
        int waveIndex = waveNumber - startWave+1;
        if (waveIndex < 0 || waveIndex >= waves.Count) return;

        StartCoroutine(SpawnWave(waves[waveIndex]));
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
