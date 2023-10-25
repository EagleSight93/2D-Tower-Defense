using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BezierPath : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform startControl;
    [SerializeField] Transform endControl;

    [Min(1)] [SerializeField] int numPoints = 10;
    [SerializeField] GameObject pointPrefab;

    List<GameObject> _currentPoints = new();

    public Vector3 GetPointOnBezier(float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * startPoint.position; // (1-t)^3 * P0
        p += 3 * uu * t * startControl.position; // 3(1-t)^2 * t * P1
        p += 3 * u * tt * endControl.position; // 3(1-t) * t^2 * P2
        p += ttt * endPoint.position; // t^3 * P3

        return p;
    }

    private void Start()
    {
        startPoint.position = MainCamera.Instance.RandomBoundsRadiusPos;
        endPoint.position = Vector3.zero;

        Tuple<Vector3, Vector3> controlPositions = GetControlPositions(startPoint.position, MainCamera.Instance.CamBounds);
        startControl.position = controlPositions.Item1;
        endControl.position = controlPositions.Item2;



        for (int i = 0; i < numPoints; i++)
        {
            float lerp = (float)i / (float)numPoints;
            _currentPoints.Add(Instantiate(pointPrefab, GetPointOnBezier(lerp), Quaternion.identity, transform));
        }
    }

    private void Update()
    {
        int currentPointCount = _currentPoints.Count;

        // If you want more points, add them
        while (currentPointCount < numPoints)
        {
            float lerp = (float)currentPointCount / (float)numPoints;
            Vector3 newPosition = GetPointOnBezier(lerp);
            GameObject newPoint = Instantiate(pointPrefab, newPosition, Quaternion.identity, transform);
            _currentPoints.Add(newPoint);
            currentPointCount++;
        }

        // If you want fewer points, destroy the excess ones
        while (currentPointCount > numPoints)
        {
            int lastIndex = currentPointCount - 1;
            Destroy(_currentPoints[lastIndex]);
            _currentPoints.RemoveAt(lastIndex);
            currentPointCount--;
        }

        // Update the positions of the existing points
        for (int i = 0; i < currentPointCount; i++)
        {
            float lerp = (float)i / (float)numPoints;
            Vector3 newPosition = GetPointOnBezier(lerp);
            _currentPoints[i].transform.position = newPosition;
        }
    }

    Tuple<Vector3, Vector3> GetControlPositions(Vector3 start, Bounds bounds)
    {
        // x and y representing what quadrant the start pos is in
        int xDir = (int)Mathf.Sign(start.x - bounds.center.x);
        int yDir = (int)Mathf.Sign(start.y - bounds.center.y);

        // the extents of the bounds on the respective side
        float xExtents = bounds.extents.x * xDir;
        float yExtents = bounds.extents.y * yDir;

        // random position in same quadrant as start pos
        float randX = Random.Range(xExtents + bounds.center.x, bounds.center.x);
        float randY = Random.Range(yExtents + bounds.center.y, bounds.center.y);
        Vector3 firstPos = new(randX, randY);

        // either start quadrant or adjadcent quadrant
        Vector3 secondDir = Random.Range(0, 2) == 0 ? new(-xDir, yDir) : new(xDir, -yDir);
        float xExtentsSecond = bounds.extents.x * secondDir.x;
        float yExtentsSecond = bounds.extents.y * secondDir.y;

        // random position in either start or adjacent quadrant
        float secondPosX = Random.Range(xExtentsSecond + bounds.center.x, bounds.center.x);
        float secondPosY = Random.Range(yExtentsSecond + bounds.center.y, bounds.center.y);
        Vector3 secondPos = new(secondPosX, secondPosY);

        return new Tuple<Vector3, Vector3>(firstPos, secondPos);
    }
}