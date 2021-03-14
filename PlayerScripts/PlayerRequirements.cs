using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerRequirements : MonoBehaviour
{
    //Public
    public KeyCode cameraKey, frontKey, backKey, rightKey, leftKey, jumpKey, runningKey;

    //Private

    //External
    public Camera _Camera;
    private ScriptableObject _CodCamera, _CodMovement;

    void Awake()
    {
        Instantiate(_Camera, new Vector3(0, transform.position.y + 1, transform.position.z - 5), Quaternion.identity);
    }

    void Start()
    { 
        transform.tag = "Player";
    }
}
