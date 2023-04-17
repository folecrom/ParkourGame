// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Grip : MonoBehaviour {
//     [Header("References")]
//     public fpc pm;
//     public Transform orientation;
//     public Transform cam;
//     public Rigidbody rb;

//     [Header("Grip Grabbing")]
//     public float moveToGripSpeed;
//     public float maxGripGrabDistance;
//     public float minTimeOnGrip;
//     private float timeOnGrip;
//     public bool holdGrip;

//     [Header("Grip Jumping")]
//     public KeyCode jumpKey = KeyCode.Space;
//     public float gripJumpForwardForce;
//     public float gripJumpUpwardForce;

//     [Header("Grip Detection")]
//     public float gripDetectionLength;
//     public float gripSphereCastRadius;
//     public LayerMask WhatIsGrip;
//     private Transform lastGrip;
//     private Transform currentGrip;
//     private RaycastHit gripHit;

//     [Header("Exiting")]
//     public bool exitingGrip;
//     public float exitGripTime;
//     public float exitGripTimer;

//     private void start() {
//         rb = GetComponent<Rigidbody>();
//         pm = GetComponent<fpc>();
//     }
//     private void Update() {
//         GripDetection();
//         StateMachine();
//         FreezeBodyOnGrip();
//     }

//     private void StateMachine() {
//         float horizontalInput = Input.GetAxisRaw("Horizontal");
//         float verticalInput = Input.GetAxisRaw("Vertical");
//         bool KeyInputPressed = horizontalInput != 0 || verticalInput != 0!;

//         /// 1st State : Holding on a grip
//         if(holdGrip)
//             {
//                 FreezeBodyOnGrip();

//                 timeOnGrip += Time.deltaTime;
//                 if(timeOnGrip > minTimeOnGrip && KeyInputPressed) EnterGripHold();

//                 if(Input.GetKeyDown(jumpKey)) GripJump();
//             }
//         /// 2nd State : Exiting a grip
//         else if(exitingGrip) {
//             if(exitGripTimer > 0) exitGripTimer -= Time.deltaTime;
//             else { exitingGrip = false;}
//         }
//     }


//     private void GripDetection() {
//         bool gripDetected = Physics.SphereCast(transform.position, gripSphereCastRadius, cam.forward, out gripHit, gripDetectionLength, WhatIsGrip);
//         if(!gripDetected) return;
        
//         float distanceToGrip = Vector3.Distance(transform.position, gripHit.transform.position);
//         if (gripHit.transform == lastGrip) return;

//         if(distanceToGrip < maxGripGrabDistance && !holdGrip) EnterGripHold();

//     }
//     private void GripJump() {
//         ExitGripHold();
//         Invoke(nameof(DelayedJumpForce), 0.05f);
//     }
//     private void DelayedJumpForce() {
//         Vector3 forceToAdd = cam.forward * gripJumpForwardForce + orientation.up * gripJumpUpwardForce;
//         rb.velocity = Vector3.zero;
//         rb.AddForce(forceToAdd, ForceMode.Impulse);
//     }
//     private void EnterGripHold() {
//         holdGrip = true;
//         pm.unlimited = true;
//         pm.restricted = true;
//         currentGrip = gripHit.transform;
//         lastGrip = gripHit.transform;
        
//         rb.useGravity = false;
//         rb.velocity = Vector3.zero;
//     }
//     private void FreezeBodyOnGrip() {
//         rb.useGravity = false;

//         Vector3 directionToGrip = currentGrip.position - transform.position;
//         float distanceToGrip = Vector3.Distance(transform.position, currentGrip.position);
//         /// Grab player towards the grip
//         if(distanceToGrip > 1f){
//             if(rb.velocity.magnitude < moveToGripSpeed) {
//                 rb.AddForce(directionToGrip.normalized * moveToGripSpeed * 1000f * Time.deltaTime);
//             }
//         }
//         /// Holding player on grip
//         else {
//             if(!pm.freeze) pm.freeze = true;
//             if(pm.unlimited) pm.unlimited = false;
//         }
//         /// Exiting if an issue happens
//         if(distanceToGrip > maxGripGrabDistance) ExitGripHold();
//     }
//     private void ExitGripHold() {
//         holdGrip = false;
//         pm.restricted = false;
//         timeOnGrip = 0f;
//         pm.freeze = false;
//         rb.useGravity = true;
//         exitingGrip = true;
//         exitGripTimer = exitGripTime;

//         StopAllCoroutines();
//         Invoke(nameof(ResetLastGrip), 1f);
//     }
//     private void ResetLastGrip() {
//         lastGrip = null;
//     }





// }