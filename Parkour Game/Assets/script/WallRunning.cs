using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("References")]
    public Transform orientation;
    private fpc pm;
    private Rigidbody rb;
    public Grip gp;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<fpc>();
        gp = GetComponent<Grip>();
    }

    private void Update(){
        CheckForWall();
        StateMachine();
    }
    private void FixedUpdate() {
        if(pm.wallRunning) {
            WallRunningMovement();
        }
    }

private void CheckForWall() {
    wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
    wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
}

private bool AboveGround() {
    return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
}

private void StateMachine() {
    /// Get Inputs
    horizontalInput = Input.GetAxisRaw("Horizontal");
    verticalInput = Input.GetAxisRaw("Vertical");

    upwardsRunning = Input.GetKey(upwardsRunKey);
    downwardsRunning = Input.GetKey(downwardsRunKey);

    /// 1st State : Wall running
    if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall) {
        // wall run call here !
        if(!pm.wallRunning) {
            StartWallRun();

            // Wall Jump call here
            if(Input.GetKeyDown(jumpKey)) WallJump();
        }
    }
    
    /// 2nd State : Exiting the wall
    else if(exitingWall) {
        if(pm.wallRunning) StopWallRun();

        if(exitWallTimer > 0) exitWallTimer -= Time.deltaTime;

        if(exitWallTimer <= 0) exitingWall = false;
    }


    /// 3rd State : out of wall
    else {
        if(pm.wallRunning) {
            StopWallRun();
        }
    }
}

private void StartWallRun() {
    pm.wallRunning = true;
}
private void WallRunningMovement() {
    if(gp.holdGrip || gp.exitingGrip) return;           /// Avoid wallRunning while grabbing grips
    rb.useGravity = false;
    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
    Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

    //applying Wallrun Movement to Forward direction of the player
    if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude) {
        wallForward = -wallForward;
    }

    /// Forward force to walk properly on wall
    rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

    /// upwards and downwards forces
    if(upwardsRunning) {
        rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
    }
    if(downwardsRunning) {
        rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);
    }

    /// push player against Wall for external walls issues
    if(!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0)) {
        rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }
}
private void StopWallRun() {
    pm.wallRunning = false;
}

private void WallJump() {
    /// Entering " exiting wall" state
    exitingWall = true;
    exitWallTimer = exitWallTime;
    
    Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
    Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

    /// reset y velocity and add force
    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    rb.AddForce(forceToApply, ForceMode.Impulse);
}




















}