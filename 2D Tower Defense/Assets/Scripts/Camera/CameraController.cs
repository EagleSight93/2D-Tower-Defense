using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] float maxPanSpeed;
    [SerializeField] float verticalPanArea;
    [SerializeField] float horizontalPanArea;
    [SerializeField] float minZoom, maxZoom;

    void Update()
    {
        if (!Application.isFocused) return;

        MainCamera mainCam = MainCamera.Instance;
        float orthographicSize = mainCam.Cam.orthographicSize;

        //Zoom
        mainCam.Cam.orthographicSize = Mathf.Clamp(-Input.mouseScrollDelta.y + orthographicSize, minZoom, maxZoom);
        
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
