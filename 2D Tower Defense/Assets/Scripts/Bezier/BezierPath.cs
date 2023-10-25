using System;
using System.Collections.Generic;
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

    List<Vector3> _points = new();

    void Start()
    {
        Vector3 target = Vector3.zero;
        Vector3 start = MainCamera.Instance.RandomBoundsRadiusPos;
        Vector3 startControl = RandomPosInRadius(start, minControlDist, maxControlDist);

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

            if (Vector3.Distance(start, target) <= 2f)
            {
                startControl = RandomPosInRadius(start, minControlDist, maxControlDist);
                CreateBezier(start, target, startControl, startControl, resolution);
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
        Transform curveContainer = new GameObject("BezierCurve").transform;
        Transform controlsContainer = new GameObject("Controls").transform;
        Transform pointsContainer = new GameObject("Points").transform;

        curveContainer.SetParent(transform);
        controlsContainer.SetParent(curveContainer);
        pointsContainer.SetParent(curveContainer);

        CreateControlPoints(startControl, endControl, controlsContainer);

        List<Vector3> bezier = BezierCurve.CreatePath(start, end, startControl, endControl, numPoints);
        foreach (Vector3 point in bezier)
        {
            Instantiate(pointPrefab, point, Quaternion.identity, pointsContainer);
        }

        _points.AddRange(bezier);
        return bezier[^1];
    }

    void CreateControlPoints(Vector3 startControl, Vector3 endControl, Transform parent)
    {
        GameObject start = Instantiate(pointPrefab, startControl, Quaternion.identity, parent);
        GameObject end = Instantiate(pointPrefab, endControl, Quaternion.identity, parent);
        start.GetComponent<SpriteRenderer>().color = Color.green;
        end.GetComponent<SpriteRenderer>().color = Color.red;
    }
}