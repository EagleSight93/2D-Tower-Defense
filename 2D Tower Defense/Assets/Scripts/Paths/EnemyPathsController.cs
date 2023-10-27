using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathsController : MonoBehaviour
{
    readonly List<EnemyPath> currentPaths = new();

    [SerializeField] float worldSize = 1000;

    [Min(1)][SerializeField] int pathsCount = 8;
    [SerializeField] WaypointPath pathPrefab;
    [SerializeField] EnemyPath enemyPathPrefab;

    private void Start()
    {
        Vector3[] points = GetPointsAroundCenter(worldSize, pathsCount);
        foreach (var point in points)
        {
            EnemyPath enemyPath = Instantiate(enemyPathPrefab, transform, false);
            WaypointPath path = Instantiate(pathPrefab, enemyPath.transform, false);
            enemyPath.path = path;
            path.CreatePathToTarget(point, Vector3.zero);
            currentPaths.Add(enemyPath);
        }
    }

    static Vector3[] GetPointsAroundCenter(float radius, int count)
    {
        Vector3[] points = new Vector3[count];
        float angleDiff = 360f / count;
        float angle = Random.Range(0f, 360f);
        for (int i = 0; i < count; i++)
        {
            angle += angleDiff;
            Vector3 newPos = (Quaternion.Euler(0, 0, angle) * Vector3.right) * radius;
            points[i] = newPos;
        }
        return points;
    }

    public EnemyPath GetPath(int index)
    {
        return currentPaths[index];
    }
}
