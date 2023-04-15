using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour 
{
    [Header("References")]
    public Transform orientation;
    public Transform Adam;
    private Rigidbody rb;
    private fpc pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;
    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;
    private bool sliding;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<fpc>();

        startYScale = Adam.localScale.y;
    }

private void Update() {
    horizontalInput = Input.GetAxisRaw("Horizontal");
    verticalInput = Input.GetAxisRaw("Vertical");

    if(Input.GetKeyDown(slideKey) && (horizontalInput !=0 || verticalInput !=0)) {
        StartSlide();
        }
        
    if (Input.GetKeyUp(slideKey) && sliding) {
        StopSlide();
    }
}

private void FixedUpdate() {
    if(sliding) {
        SlidingMouvement();
    }

}



private void StartSlide() {
    sliding = true;

    Adam.localScale = new Vector3(Adam.localScale.x, slideYScale, Adam.localScale.z);
    rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

    slideTimer = maxSlideTime;
}

private void SlidingMouvement() {
    Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    /// normal slide
    if (!pm.OnSlope() || rb.velocity.y > -0.1f) {
        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

        slideTimer -= Time.deltaTime;
    }
    ///Sliding down on a slop surface
    else {
        rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
    }
    if (slideTimer <= 0) {
        StopSlide();
    }
}

private void StopSlide() {
    sliding = false;

    Adam.localScale = new Vector3(Adam.localScale.x, startYScale, Adam.localScale.z);
}

}