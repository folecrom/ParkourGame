using UnityEngine;
using UnityEngine.InputSystem;


public class Slidingmovement : MonoBehaviour
{

public float tailleController =2f;
public float slideTimerMax;
public float slideTimer;
public float slidespeed;
public Cameraplayer VueJoueur;

public CharacterController  _characterController;

private void Awake() {
    _characterController = GetComponent<CharacterController>();
}

private void Update(){
if (Input.GetKey(KeyCode.LeftControl))
    {
        StartSlide();
    }
    else 
    {
        StopSlide();
    }
}

private void StartSlide(){
    _characterController.height = 0.5f;
    
}
private void StopSlide(){
    _characterController.height = tailleController;
}
private bool IsGrounded() => _characterController.isGrounded;
}
