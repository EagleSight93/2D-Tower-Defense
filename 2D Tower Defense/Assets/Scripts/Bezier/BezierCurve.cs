using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    public static Vector3 GetPointOnBezier(Vector2 start, Vector2 end, Vector2 startControl, Vector2 endControl, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 p = uuu * start; // (1-t)^3 * P0
        p += 3 * uu * t * startControl; // 3(1-t)^2 * t * P1
        p += 3 * u * tt * endControl; // 3(1-t) * t^2 * P2
        p += ttt * end; // t^3 * P3

        return p;
    }

    public static List<Vector3> CreatePath(Vector3 start, Vector3 end, Vector3 startControl, Vector3 endControl, int numPoints)
    {
        List<Vector3> curPoints = new();
        for (int i = 0; i < numPoints; i++)
        {
            float lerp = (float)i / numPoints;
            Vector2 pos = GetPointOnBezier(start, end, startControl, endControl, lerp);
            curPoints.Add(pos);
        }
        return curPoints;
    }
}
