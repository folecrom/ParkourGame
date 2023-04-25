using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

////////********* 2nd VERSION ==> with rigibody 3D component ******//////

public class fpc : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallRunSpeed;

    private float bestMoveSpeed;
    private float lastBestMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        wallRunning,
        air
    }
    public bool freeze;
    public bool unlimited;
    public bool restricted;
    public bool sliding;
    public bool wallRunning;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

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

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // /// State when Freezing player
        // if(freeze) {
        //     state = MovementState.freeze;
        //     rb.velocity = Vector3.zero;
        // }
        // /// State when unlimited speed is applied to player
        // if(unlimited) {
        //     state = MovementState.unlimited;
        //     moveSpeed = 969f;
        //     return;
        // }
        /// State when Wall Running
        if(wallRunning) {
            state = MovementState.wallRunning;
            bestMoveSpeed = wallRunSpeed;
        }
        /// State when Sliding
        if(sliding) {
            state = MovementState.sliding;
            if (OnSlope() && rb.velocity.y < 0.1f) {
                bestMoveSpeed = slideSpeed;
            }
            else {
                bestMoveSpeed = sprintSpeed;
            }
        }
        /// State when Crounching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            bestMoveSpeed = crouchSpeed;
        }

        /// State Sprinting
        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            bestMoveSpeed = sprintSpeed;
        }

        /// State walking
        else if (grounded)
        {
            state = MovementState.walking;
            bestMoveSpeed = walkSpeed;
        }

        /// State in the air
        else
        {
            state = MovementState.air;
        }

        /// check if bestMoveSpeed has a significant change
        if(Mathf.Abs(bestMoveSpeed - lastBestMoveSpeed) > 4f && moveSpeed != 0) {
            StopAllCoroutines();
            StartCoroutine(SmoothMoveSpeed());
        }
        else {
            moveSpeed = bestMoveSpeed;
        }

        lastBestMoveSpeed = bestMoveSpeed;
    }

    private IEnumerator SmoothMoveSpeed() {

        /// slowly decrease speed after building some through slopes
        float time = 0;
        float difference = Mathf.Abs(bestMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference) {
            moveSpeed = Mathf.Lerp(startValue, bestMoveSpeed, time / difference);
            
            if (OnSlope()) {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle/ 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else{
                time += Time.deltaTime * speedIncreaseMultiplier;
            }
            yield return null;
        }

        moveSpeed = bestMoveSpeed;
    }
    private void MovePlayer()
    {
        // Remove player Inputs when Unlimited is active
        if(restricted) return;
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && exitingSlope == false)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(grounded == false)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && exitingSlope == false)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}























































































/************* FIRST VERSION ==> Character Controller , NOT ADAPTED WITH EXPECTED GAMEPLAY ************////////

public class fpc : MonoBehaviour
{
    [SerializeField] private float gravityMultiplier = 1.0f;
    [SerializeField] private float jumpPower = 3.0f;
    [SerializeField] private float _speed = 2.0f;

    [SerializeField] private float _runSpeed = 3.0f;
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
    //[SerializeField] private bool WillSlideOnSlopes = true;
   //[SerializeField] private float slopeSpeed = 8f;
    private Vector3 hitPointNormal;
    /*private bool IsSliding {
    get{
        if(_characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
        {
            hitPointNormal = slopeHit.normal;
            return Vector3.Angle(hitPointNormal, Vector3.up)>_characterController.slopeLimit;
        }
        else
        {
            return false;
        }
    }
}*/

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        CameraMouse();
        ApplyMovement();
        ApplyGravity();
    }

    private void FixedUpdate() {
        _direction = new Vector3(_input.x, 0.0f, _input.y);
        _direction = transform.TransformDirection(_direction)*_speed;
    }
    public void Move(InputAction.CallbackContext context)
    {  
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
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
            _speed = 3.0f;
        }
    }
    private bool IsGrounded() => _characterController.isGrounded;


    
}