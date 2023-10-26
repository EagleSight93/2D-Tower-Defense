using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraBoundsUpdater : MonoBehaviour
{
    BoxCollider2D _boxCol;
    Camera _cam;

    private void Awake()
    {
        _boxCol = GetComponent<BoxCollider2D>();
        _cam = GetComponent<Camera>();
    }
    private void Update()
    {
        float height = _cam.orthographicSize * 2;
        float width = height * _cam.aspect;

        _boxCol.offset = Vector3.zero;
        _boxCol.size = new Vector2(width, height);
    }
}
