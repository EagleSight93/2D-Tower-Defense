using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BezierPath : MonoBehaviour
{
    [SerializeField] int resolution = 100;
    [SerializeField] float minControlDist = 0f;
    [SerializeField] float maxControlDist = 4f;
    [SerializeField] float distToNextPath = 3f;

    [SerializeField] GameObject pointPrefab;

    readonly List<Vector3> _points = new();
    LineRenderer _lr;

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
    }

    public void CreatePathToTarget(Vector3 start, Vector3 target)
    {
        int iterations = 0;
        while (start != target)
        {
            iterations++;
            if (iterations >= 100)
            {
                Debug.LogError("Infinite loop");
                break;
            }

            Vector3 end = start + (target - start).normalized * distToNextPath;
            Vector3 startControl;
            if (Vector3.Distance(start, target) <= 2f)
            {
                startControl = RandomPosInRadius(start, minControlDist, maxControlDist);
                CreateBezier(start, target, startControl, startControl, resolution);

                _lr.positionCount = _points.Count;
                _lr.SetPositions(_points.ToArray());
                return;
            }

            startControl = RandomPosInRadius(start, minControlDist, maxControlDist);
            start = CreateBezier(start, end, startControl, startControl, resolution);
        }
    }


    Vector3 RandomPosInRadius(Vector2 start, float min, float max)
    {
        float randDist = Random.Range(min, max);
        return (Random.insideUnitCircle * randDist) + start;
    }

    Vector3 CreateBezier(Vector3 start, Vector3 end, Vector3 startControl, Vector3 endControl, int numPoints)
    {
        List<Vector3> bezier = BezierCurve.CreatePath(start, end, startControl, endControl, numPoints);
        _points.AddRange(bezier);
        return bezier[^1];
    }
}