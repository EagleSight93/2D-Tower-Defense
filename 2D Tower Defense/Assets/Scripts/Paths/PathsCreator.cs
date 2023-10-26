using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsCreator : MonoBehaviour
{
    [Min(1)][SerializeField] int radiusPointsCount = 8;
    [SerializeField] float radiusDistOffset = 1.5f;
    [SerializeField] BezierPath pathPrefab;

    private void Start()
    {
        Vector3[] points = GetPointsAroundCenter(MainCamera.Instance.CamBounds.max.x + radiusDistOffset, radiusPointsCount);
        foreach (var point in points)
        {
            BezierPath path = Instantiate(pathPrefab, transform, false);
            path.CreatePathToTarget(point, Vector3.zero);
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
            newPos = MainCamera.Instance.CamBounds.ClosestPoint(newPos);
            points[i] = newPos;
        }
        return points;
    }
}
