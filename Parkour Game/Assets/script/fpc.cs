using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class fpc : MonoBehaviour
{
    [SerializeField] private float gravityMultiplier = 1.0f;
    [SerializeField] private float jumpPower = 3.0f;
    [SerializeField] private float _speed = 2.0f;

    [SerializeField] private float _runSpeed = 5.0f;
    //appel du CharacterController
    private CharacterController _characterController;
    // variable pour la camera
    public Camera playerCamera;
    private float rotationx;
    private float rotationy;
    public float mouseSensitivty = 3f;
    public float maxViewAngle = 90f;
    private Vector2 _input;
    private Vector3 _direction;
    private float _velocity;
    private float _gravity = -9.81f;

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        CameraMouse();
        ApplyMovement();
        ApplyGravity();
        IsGrounded();
    }

    private void FixedUpdate() {
        _direction = new Vector3(_input.x, 0.0f, _input.y);
        _direction = transform.TransformDirection(_direction)*_speed;
        Vector3 _directionChange = _direction * Time.deltaTime;
        _characterController.Move(_directionChange * Time.deltaTime);
    }
    public void Deplacements(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }
    public void CameraMouse(){
        rotationx = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivty;
        rotationy = rotationy - Input.GetAxis("Mouse Y")* mouseSensitivty;

        rotationy = Mathf.Clamp(rotationy, -maxViewAngle, maxViewAngle);

        transform.localEulerAngles = new Vector3(0, rotationx, 0);
        playerCamera.transform.localEulerAngles = new Vector3(rotationy, 0, 0);
    }
    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity=-1.0f;
        }
        else{
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
        _direction.y = _velocity;
    }
    private void ApplyMovement()
    {
        _characterController.Move(_direction * _speed * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started) return;
        if (!IsGrounded()) return;

        _velocity = jumpPower ;
    }
    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
        _speed = _runSpeed;
        }
        else if (context.canceled)
        {
            _speed = 2.0f;
        }
    }
    private bool IsGrounded() => _characterController.isGrounded;
}