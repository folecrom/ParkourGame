using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

////////********* 2nd VERSION ==> with rigibody 3D component ******//////


public class fpc : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
























































































///************* FIRST VERSION ==> Character Controller , NOT ADAPTED WITH EXPECTED GAMEPLAY ************////////

// public class fpc : MonoBehaviour
// {
//     [SerializeField] private float gravityMultiplier = 1.0f;
//     [SerializeField] private float jumpPower = 3.0f;
//     [SerializeField] private float _speed = 2.0f;

//     [SerializeField] private float _runSpeed = 3.0f;
//     //appel du CharacterController
//     private CharacterController _characterController;
//     // variable pour la camera
//     public Camera playerCamera;
//     private float rotationx;
//     private float rotationy;
//     public float mouseSensitivty = 3f;
//     public float maxViewAngle = 90f;
//     private Vector2 _input;
//     private Vector3 _direction;
//     private float _velocity;
//     private float _gravity = -9.81f;
//     //[SerializeField] private bool WillSlideOnSlopes = true;
//    //[SerializeField] private float slopeSpeed = 8f;
//     private Vector3 hitPointNormal;
//     /*private bool IsSliding {
//     get{
//         if(_characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
//         {
//             hitPointNormal = slopeHit.normal;
//             return Vector3.Angle(hitPointNormal, Vector3.up)>_characterController.slopeLimit;
//         }
//         else
//         {
//             return false;
//         }
//     }
// }*/

//     private void Awake() {
//         _characterController = GetComponent<CharacterController>();
//         Cursor.lockState = CursorLockMode.Locked;
//     }

//     private void Update() {
//         CameraMouse();
//         ApplyMovement();
//         ApplyGravity();
//     }

//     private void FixedUpdate() {
//         _direction = new Vector3(_input.x, 0.0f, _input.y);
//         _direction = transform.TransformDirection(_direction)*_speed;
//     }
//     public void Move(InputAction.CallbackContext context)
//     {  
//         _input = context.ReadValue<Vector2>();
//         _direction = new Vector3(_input.x, 0.0f, _input.y);
//     }
//     public void CameraMouse(){
//         rotationx = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivty;
//         rotationy = rotationy - Input.GetAxis("Mouse Y")* mouseSensitivty;

//         rotationy = Mathf.Clamp(rotationy, -maxViewAngle, maxViewAngle);

//         transform.localEulerAngles = new Vector3(0, rotationx, 0);
//         playerCamera.transform.localEulerAngles = new Vector3(rotationy, 0, 0);
//     }
//     private void ApplyGravity()
//     {
//         if (IsGrounded() && _velocity < 0.0f)
//         {
//             _velocity=-1.0f;
//         }
//         else{
//             _velocity += _gravity * gravityMultiplier * Time.deltaTime;
//         }
//         _direction.y = _velocity;
//     }
//     private void ApplyMovement()
//     {
//         _characterController.Move(_direction * _speed * Time.deltaTime);
//     }

//     public void Jump(InputAction.CallbackContext context)
//     {
//         if (context.started) return;
//         if (!IsGrounded()) return;

//         _velocity = jumpPower ;
//     }
//     public void Run(InputAction.CallbackContext context)
//     {
//         if (context.performed)
//         {
//         _speed = _runSpeed;
//         }
//         else if (context.canceled)
//         {
//             _speed = 3.0f;
//         }
//     }
//     private bool IsGrounded() => _characterController.isGrounded;


    
// }