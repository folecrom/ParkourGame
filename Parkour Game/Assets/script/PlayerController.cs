using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;

    private Vector3 _direction;
    private float _velocity;
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 1.0f;
    [SerializeField] private float jumpPower = 3.0f;
    [SerializeField] private float _speed = 5.0f;

    [SerializeField] private float _runSpeed = 10.0f;


    private void Awake() {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        ApplyMovement();
        ApplyGravity();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
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
        if (!context.started) return;
        if (!IsGrounded()) return;

        _velocity = jumpPower;
    }
    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
        _speed = _runSpeed;
        }
        else if (context.canceled)
        {
            _speed = 5.0f;
        }
    }
    private bool IsGrounded() => _characterController.isGrounded;
}
    