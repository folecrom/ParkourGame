using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunWAll : MonoBehaviour
{
    [Header("WAllRunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
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
    private PlayerMovementAdvanced pm ;
    private Rigidbody rb;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
    }

    void Update(){
        CheckForWall();
    }

    private void CheckForWall(){
        wallRight = Physics.Raycast(transform.position, orientation.right,out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right,out leftWallhit, wallCheckDistance, whatIsWall);
    }
}
