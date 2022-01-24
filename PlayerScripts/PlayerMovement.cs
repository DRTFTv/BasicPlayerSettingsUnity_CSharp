using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Public
    public float walkingSpeed = 250.0f, runningSpeed = 750.0f, jumpForce = 25000;

    //Private
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rigidbody;

    //External
    public Camera _Camera;

    void Start()
    {
        if (_Camera == null)
            _Camera = Camera.main;

        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        Vector3 forwardDirection = new Vector3(_Camera.transform.forward.x, 0, _Camera.transform.forward.z);
        forwardDirection.Normalize();
        forwardDirection *= Input.GetAxis("Vertical");

        Vector3 sideDirection = new Vector3(_Camera.transform.right.x, 0, _Camera.transform.right.z);
        sideDirection.Normalize();
        sideDirection *= Input.GetAxis("Horizontal");

        Vector3 finalDirection = forwardDirection + sideDirection;
        if (finalDirection.sqrMagnitude > 1)
            finalDirection.Normalize();

        moveDirection = new Vector3(finalDirection.x, 0, finalDirection.z);

        if (Input.GetKey(gameObject.GetComponent<PlayerRequirements>().runningKey))
            moveDirection *= runningSpeed * Time.deltaTime;
        else
            moveDirection *= walkingSpeed * Time.deltaTime;

        rigidbody.velocity = new Vector3(moveDirection.x, rigidbody.velocity.y, moveDirection.z);

        if (Physics.Raycast(transform.position, -Vector3.up, 1.10f))
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.AddForce(Vector3.up * jumpForce);
            }
    }
}
