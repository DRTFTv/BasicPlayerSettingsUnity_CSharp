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
    [Range(1.0f, 5.0f)] public float sensiCamX = 1.0f;
    [Range(0.1f, 1.0f)] public float sensiCamY = 1.0f;
    [Range(0.01f, 1.0f)] public float smoothCam = 1.0f;
    [Range(0.0f, 5.0f)] public float zoomCam = 3;

    //Private
    private Vector3 offsetCam;
    private float camX, camY;
    private RaycastHit hit = new RaycastHit();

    //External
    public GameObject _Player;
    private Transform _PT;
    private Rigidbody _PRB;

    void Start()
    {
        if (_Player == null)
            _Player = GameObject.FindGameObjectWithTag("Player");

        _PT = _Player.GetComponent<Transform>();
        _PRB = _Player.GetComponent<Rigidbody>();
        offsetCam = transform.position - _PT.position;
    }

    void LateUpdate()
    {
        if (mouseOption == MouseOption.locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            camX = Input.GetAxis("Mouse X") * sensiCamX;

            camY = Mathf.Clamp(camY -= Input.GetAxis("Mouse Y") * sensiCamY, -1.0f, 5.0f);

            zoomCam = Mathf.Clamp(zoomCam -= Input.mouseScrollDelta.y, 0.0f, 5.0f);

            Quaternion angleCam = Quaternion.AngleAxis(camX, Vector3.up);
            offsetCam = angleCam * offsetCam;
            Vector3 newPos = _PT.position + offsetCam;
            newPos.y += camY;
            newPos -= transform.forward * zoomCam;
            transform.position = Vector3.Slerp(transform.position, newPos, smoothCam);
            transform.LookAt(_PT);

            if (_PRB.velocity != Vector3.zero)
            {
                _PRB.transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            }
            else
            {
                _PRB.transform.forward = _PRB.transform.forward;
            }

            if (Physics.Linecast(_PT.position, transform.position, out hit))
                transform.position = hit.point + transform.forward * 1.0f;
        }
        else if(mouseOption == MouseOption.confined)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
