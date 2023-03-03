using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;

    private Vector3 _direction;

    [SerializeField] private float _speed = 5.0f;

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        _characterController.Move(_direction * _speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }
}
