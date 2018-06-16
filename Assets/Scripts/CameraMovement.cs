using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private const float Y_ANGLE_MIN = -360.0f;
    private const float Y_ANGLE_MAX = 360.0f;

    public Transform lookAt;
    public Transform camTansfrom;
    private Transform camTransform;
    private Camera cam;

    public float turnSpeed = 4.0f;
    private float panSpeed = 4.0f;
    public float zoomSpeed = 4.0f;

    private float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f; 
    private float sensivityX = 4.0f;
    private float sensivityY = 1.0f;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

   

    private void Update()
    {

        if (Input.GetMouseButton(1))
        {
        currentX -= Input.GetAxis("Mouse Y")*turnSpeed;
        currentY += Input.GetAxis("Mouse X")*turnSpeed;
        }

        distance -= Input.GetAxis("Mouse ScrollWheel")*zoomSpeed;
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentX, currentY, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
