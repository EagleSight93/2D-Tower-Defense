using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [HideInInspector] public BezierPath path;
    EnemyWaveManager _waveManager;

    private void Awake()
    {
        _waveManager = FindObjectOfType<EnemyWaveManager>();
    }

    private void Start()
    {
        SpawnWave(groupCount: 10);
    }

    public void SpawnWave(int groupCount)
    {
        StartCoroutine(SpawnWaveRoutine(groupCount));
    }

    IEnumerator SpawnWaveRoutine(int groupCount)
    {
        for(int i = 0; i < groupCount; i++)
        {
            yield return SpawnGroupRoutine();
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    IEnumerator SpawnGroupRoutine()
    {
        int randIndex = Random.Range(0, _waveManager.possibleGroups.Count);
        EnemyGroupSO enemyGroup = _waveManager.possibleGroups[randIndex];
        foreach (Enemy enemyPrefab in enemyGroup.enemies)
        {
            Enemy enemy = Instantiate(enemyPrefab, path.Points[0], Quaternion.identity);
            enemy.pathfinding.Init(path.Points);
            yield return new WaitForSeconds(enemyGroup.spawnInterval);
        }
    }
}
