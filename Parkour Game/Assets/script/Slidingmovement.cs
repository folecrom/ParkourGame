using UnityEngine;
using UnityEngine.InputSystem;


public class Slidingmovement : MonoBehaviour
{
[SerializeField] private bool WillSlideOnSlopes = true;
[SerializeField] private float slopeSpeed = 8f;
public Vector3 hitPointNormal;
public Vector3 _direction;
public Vector2 _input;
public bool IsSliding {
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
}
public float tailleController =2f;
public float slideTimerMax;
public float slideTimer;
public float slidespeed;
public Cameraplayer VueJoueur;
private fpc _fpc;

public CharacterController  _characterController;

private void Awake() {
    _characterController = GetComponent<CharacterController>();
    _fpc = GetComponent<fpc>();
    _direction = new Vector3(_input.x, 0.0f, _input.y);
}

private void Update(){
    
}

public void StartSlide(){
    _characterController.height = 0.5f;
}
public void StopSlide(){
    _characterController.height = tailleController;
}
private bool IsGrounded() => _characterController.isGrounded;
}
