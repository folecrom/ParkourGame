using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Vector2 _input;
    private CharacterController _characterController;

    public Vector3 _direction;
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
        public CharacterController controller; // référence au Character Controller
    public float slideSpeed = 5f; // vitesse de glissement
    public float slideDuration = 1f; // durée de glissement
    private float slideTime = 0f; // temps de glissement écoulé
    private bool isSliding = false; // est-ce que le personnage est en train de glisser

    public void Slide(InputAction.CallbackContext context)
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) // si la touche de glissement est enfoncée
        {
            if (!isSliding) // si le personnage n'est pas déjà en train de glisser
            {
                isSliding = true;
                slideTime = 0f;
            }
        }

        if (isSliding) // si le personnage est en train de glisser
        {
            slideTime += Time.deltaTime;
            if (slideTime > slideDuration) // si la durée de glissement est écoulée
            {
                isSliding = false;
            }
            else // sinon, faire glisser le personnage
            {
                controller.Move(transform.forward * slideSpeed * Time.deltaTime);
            }
        }
    }
    private bool IsGrounded() => _characterController.isGrounded;
}
    