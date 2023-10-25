using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera Instance { get; private set; }
    public Camera Cam { get; private set; }
    public Bounds CamBounds => _boundsCol.bounds;
    public Vector2 MousePos => Cam.ScreenToWorldPoint(Input.mousePosition);
    public Vector3 ClampedMousePos
    {
        get
        {
            Vector2 mousePos = MousePos;
            float clampedX = Mathf.Clamp(mousePos.x, -CamBounds.extents.x, CamBounds.extents.x);
            float clampedY = Mathf.Clamp(mousePos.y, -CamBounds.extents.y, CamBounds.extents.y);
            Vector2 clampedPos = new Vector3(clampedX, clampedY) + CamBounds.center;
            return clampedPos;
        }
    }
    public Vector3 RandomBoundsPos 
    { 
        get
        {
            Bounds bounds = CamBounds;
            Vector2 extents = bounds.extents;
            float randX = Random.Range(-extents.x, extents.x);
            float randY = Random.Range(-extents.y, extents.y);
            Vector2 randPos = new Vector3(randX, randY) + bounds.center;
            return randPos;
        } 
    }
    public Vector3 RandomBoundsRadiusPos 
    {
        get
        {
            int randEdge = Random.Range(0, 4);
            Bounds bounds = CamBounds;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            return randEdge switch
            {
                0 => new Vector3(Random.Range(min.x, max.x), max.y, 0),
                1 => new Vector3(max.x, Random.Range(min.y, max.y), 0),
                2 => new Vector3(Random.Range(min.x, max.x), min.y, 0),
                3 => new Vector3(min.x, Random.Range(min.y, max.y), 0),
                _ => Vector3.zero
            };
        }
    }
    BoxCollider2D _boundsCol;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Mutliple Main Camera's In Scene");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Cam = GetComponent<Camera>();
        _boundsCol = GetComponent<BoxCollider2D>();
    }
}
