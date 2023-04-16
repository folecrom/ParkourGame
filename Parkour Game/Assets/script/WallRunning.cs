using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
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

    [Header("References")]
    public Transform orientation;
    private fpc pm;
    private Rigidbody rb;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<fpc>();
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
    if((wallLeft || wallRight) && verticalInput > 0 && AboveGround()) {
        // wall run call here !
        if(!pm.wallRunning) {
            StartWallRun();
        }
    }
    
    /// 3rd State : out of run
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






















}