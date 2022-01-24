using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //Public
    public enum MouseOption { locked, confined }
    public MouseOption mouseOption = MouseOption.locked;
    public enum ChosenCam { orbitalCamera, gameplayCamera }
    [Range(1.0f, 5.0f)] public float sensitivityCameraX = 1.0f;
    [Range(0.1f, 1.0f)] public float sensitivityCameraY = 1.0f;
    [Range(0.01f, 1.0f)] public float smoothCamera = 1.0f;
    [Range(0.0f, 5.0f)] public float zoomCamera = 3;

    //Private
    private Vector3 offsetCamera;
    private float cameraX, cameraY;
    private RaycastHit raycastHit = new RaycastHit();

    //External
    public GameObject _PlayerGameObject;
    private Transform _PlayerTransform;
    private Rigidbody _PlayerRigidbody;

    void Start()
    {
        if (_PlayerGameObject == null)
            _PlayerGameObject = GameObject.FindGameObjectWithTag("Player");

        _PlayerTransform = _PlayerGameObject.GetComponent<Transform>();
        _PlayerRigidbody = _PlayerGameObject.GetComponent<Rigidbody>();
        offsetCamera = transform.position - _PlayerTransform.position;
    }

    void LateUpdate()
    {
        if (mouseOption == MouseOption.locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            cameraX = Input.GetAxis("Mouse X") * sensitivitycameraX;

            cameraY = Mathf.Clamp(cameraY -= Input.GetAxis("Mouse Y") * sensitivityCameraY, -1.0f, 5.0f);

            zoomCamera = Mathf.Clamp(zoomCamera -= Input.mouseScrollDelta.y, 0.0f, 5.0f);

            Quaternion angleCam = Quaternion.AngleAxis(cameraX, Vector3.up);
            offsetCamera = angleCam * offsetCamera;
            Vector3 newPos = _PlayerTransform.position + offsetCamera;
            newPos.y += cameraY;
            newPos -= transform.forward * zoomCamera;
            transform.position = Vector3.Slerp(transform.position, newPos, smoothCamera);
            transform.LookAt(_PlayerTransform);

            if (_PlayerRigidbody.velocity != Vector3.zero)
            {
                _PlayerRigidbody.transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            }
            else
            {
                _PlayerRigidbody.transform.forward = _PlayerRigidbody.transform.forward;
            }

            if (Physics.Linecast(_PlayerTransform.position, transform.position, out raycastHit))
                transform.position = raycastHit.point + transform.forward * 1.0f;
        }
        else if(mouseOption == MouseOption.confined)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
