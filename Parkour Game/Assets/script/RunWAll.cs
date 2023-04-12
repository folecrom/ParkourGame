// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class RunWAll : MonoBehaviour
// {
//     [Header("WAllRunning")]
//     public LayerMask whatIsWall;
//     public LayerMask whatIsGround;
//     public float wallRunForce;
//     public float maxWallRunTime;
//     private float wallRunTimer;

//     [Header("Input")]
//     private float horizontalInput;
//     private float verticalInput;

//     [Header("Detection")]
//     public float wallCheckDistance;
//     public float minJumpHeight;
//     private RaycastHit leftWallhit;
//     private RaycastHit rightWallhit;
//     private bool wallLeft;
//     private bool wallRight;

//     [Header("References")]
//     public Transform orientation;
//     private PlayerMovementAdvanced pm;
//     private Rigidbody rb;

//     private void Start(){
//         rb = GetComponent<Rigidbody>();
//         pm = GetComponent<PlayerMovementAdvanced>();
//     }

//     void Update(){
//         CheckForWall();
//         StateMachine();
//     }
//     void FixedUpdate() {
//         if(pm.wallrunning) {
//             WallRunningMovement();
//         }
//     }

//     private void CheckForWall(){
//         wallRight = Physics.Raycast(transform.position, orientation.right,out rightWallhit, wallCheckDistance, whatIsWall);
//         wallLeft = Physics.Raycast(transform.position, -orientation.right,out leftWallhit, wallCheckDistance, whatIsWall);
//     }
//     private bool AboveGround() {
//         return !Physics.Raycast(transform.postion, Vector3.down, minJumpHeight, whatIsGround);
//     }
//     private void StateMachine() {
//         //getting Inputs
//         horizontalInput = Input.GetAxisRaw("Horizontal");
//         verticalInput = Input.GetAxisRaw("Vertical");

//         //State 1 : Wall Running
//         if((wallLeft || wallRight) && verticalInput > 0 && AboveGround()) {
//             if (!pm.wallrunning) {
//                 StartWallRun();
//             }
//             //State 2 - None
//             else {
//                 if (pm.wallrunning) {
//                     StopWallRun();
//                 }
//             }
//         }
//     }
//     private void StartWallRun(){
//         pm.wallrunning = true;
//     }
//     private WallRunningMovement() {
//         rb.UseGravity =false;
//         rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

//         vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
//         vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
        
//         // forward force
//         rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
//     } 
//     private void StopWallRun() {
//         rb.wallrunning = false;
//     }
// }
