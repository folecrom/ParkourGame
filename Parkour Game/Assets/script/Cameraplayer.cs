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
    
    float mouseX ;
    float mouseY ;

    float xRotation = 0f;
    Vector2 _input;
    Vector3 _direction;

    void Start()
    {
        
    }

    void Update()
    {
         mouseX = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
         mouseY = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        transform.localRotation = Quaternion.Euler(mouseX, 0f, mouseY);
        playerBody.Rotate(Vector3.up * mouseY);
    }

    void FixedUpdate()
    {
        _direction = new Vector3(_input.x, 0.0f, _input.y);
        _direction = transform.TransformDirection(_direction);

        playerBody.transform.position += _direction;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }
}
