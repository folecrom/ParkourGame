using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Cameraplayer : MonoBehaviour
{
    public float sensitivity = 100f;
    public Transform playerBody;
    public float RotationX ;
    public float RotationY ;

    Vector2 _input;
    Vector3 _direction;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
         RotationX -= Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
         RotationY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        RotationX = Mathf.Clamp(RotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(RotationX, RotationY, 0f);
        playerBody.Rotate(Vector3.up * RotationX);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
        _direction = transform.TransformDirection(_direction);

        playerBody.transform.position += _direction;
    }
}
