using System.Collections;
using System.Collections.Generic;
using Core.Logging;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera Instance { get; private set; }
    public Camera Cam { get; private set; }
    public Bounds CamBounds
    {
        get
        {
            float height = Cam.orthographicSize * 2;
            float width = height * Cam.aspect;
            Vector2 size = new(width, height);
            return new Bounds(transform.position, size);
        }
    }
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

    public float ZoomLerp => (Cam.orthographicSize - minZoom) / (maxZoom - minZoom);


    [SerializeField] float maxPanSpeed = 1f;
    [SerializeField] float zoomSpeed = 10f;
    [SerializeField] float verticalPanArea = 50f;
    [SerializeField] float horizontalPanArea = 50f;
    [SerializeField] float minZoom = 2.5f, maxZoom = 200f;

    readonly CLogger _logger = new(true);

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            _logger.LogError("Mutliple Main Camera's In Scene");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Cam = GetComponent<Camera>();
    }
    
    void Update()
    {
        if (!Application.isFocused) return;

        MainCamera mainCam = MainCamera.Instance;
        float orthographicSize = mainCam.Cam.orthographicSize;

        //Zoom
        mainCam.Cam.orthographicSize = Mathf.Clamp((-Input.mouseScrollDelta.y * zoomSpeed) + orthographicSize, minZoom, maxZoom);

        float mouseHeightInArea = 0;
        float mouseWidthInArea = 0;

        //4 ifs
        if (Input.mousePosition.y > (Screen.height - verticalPanArea))
        {
            mouseHeightInArea = verticalPanArea - (Screen.height - Input.mousePosition.y);

        }
        else if (Input.mousePosition.y <= verticalPanArea)
        {
            mouseHeightInArea = verticalPanArea - Input.mousePosition.y;
        }

        if (Input.mousePosition.x > (Screen.width - horizontalPanArea))
        {
            mouseWidthInArea = horizontalPanArea - (Screen.width - Input.mousePosition.x);

        }
        else if (Input.mousePosition.x <= horizontalPanArea)
        {
            mouseWidthInArea = horizontalPanArea - Input.mousePosition.x;
        }

        //Pan
        if (mouseHeightInArea != 0 || mouseWidthInArea != 0)
        {
            float heightPercentage = mouseHeightInArea / verticalPanArea;
            float widthPercentage = mouseWidthInArea / horizontalPanArea;

            float maxPercentage = Mathf.Max(heightPercentage, widthPercentage);

            Vector3 mouseDir = (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f)).normalized;
            transform.position += mouseDir * (maxPanSpeed * maxPercentage * Time.deltaTime * orthographicSize);
        }
    }
}
