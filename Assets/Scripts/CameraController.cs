using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController cameraController;
    
    public float panSpeed = 5;
    public float zoomedInAngle = 90;
    public float zoomedOutAngle = 45;
    public float minZoom = 20;
    public float maxZoom = 200;
    public bool inverseZoom = false;
    private float zoomLevel = 0;
    private Transform rotationObject;

    private Transform zoomObject;
    // Start is called before the first frame update
    void Awake()
    {
        cameraController = this;
        rotationObject = transform.GetChild(0);
        zoomObject = rotationObject.transform.GetChild(0);
        ResetCamera();
    }

    public void ResetCamera()
    {
        this.transform.position = new Vector3(0, 0, 0);
        zoomLevel = 0;
        rotationObject.transform.rotation = Quaternion.Euler(zoomedInAngle, 0, 0);
        zoomObject.transform.localPosition = new Vector3(0, 0, -minZoom);
    }

    void ChangePosition()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            float movementFactor = Mathf.Lerp(minZoom, maxZoom, zoomLevel);
            float distance = panSpeed * Time.deltaTime;
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            float dampingFactor =
                Mathf.Max(Mathf.Abs(Input.GetAxis("Horizontal")), Mathf.Abs(Input.GetAxis("Vertical")));
            transform.Translate(direction * dampingFactor * movementFactor * distance);
            ClampCameraPan();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeZoom();
        ChangePosition();
    }

    void ClampCameraPan()
    {
        Vector3 position = this.transform.position;

        if (Galaxy.GalaxyInstance.galaxyView == true)
        {
            position.x = Mathf.Clamp(transform.position.x, -Galaxy.GalaxyInstance.maximumRadius,
                Galaxy.GalaxyInstance.maximumRadius);
            position.z = Mathf.Clamp(transform.position.z, -Galaxy.GalaxyInstance.maximumRadius,
                Galaxy.GalaxyInstance.maximumRadius);
        }
        else
        {
            position.x = Mathf.Clamp(transform.position.x, -50, 50);
            position.z = Mathf.Clamp(transform.position.z, -50, 50);
        }

        this.transform.position = position;
    }

    void ChangeZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (!inverseZoom)
            {
                zoomLevel = Mathf.Clamp01(zoomLevel - Input.GetAxis("Mouse ScrollWheel"));
            }
            else
            {
                zoomLevel = Mathf.Clamp01(zoomLevel + Input.GetAxis("Mouse ScrollWheel"));
            }

            float zoom = Mathf.Lerp(-minZoom, -maxZoom, zoomLevel);
            zoomObject.transform.localPosition = new Vector3(0, 0, zoom);

            float zoomAngle = Mathf.Lerp(zoomedInAngle, zoomedOutAngle, zoom);
            zoomObject.transform.localRotation = Quaternion.Euler(zoomAngle, 0, 0);
        }
    }

    public void MoveTo(Vector3 pos)
    {
        this.transform.position = pos;
    }
}
