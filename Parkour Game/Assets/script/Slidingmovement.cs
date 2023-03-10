using UnityEngine;
using UnityEngine.InputSystem;


public class Slidingmovement : MonoBehaviour
{
    public CharacterController controller; // référence au Character Controller
    public float slideSpeed = 10f; // vitesse de glissement
    public float slideDuration = 1f; // durée de glissement
    private float slideTime = 0f; // temps de glissement écoulé
    private bool isSliding = false; // est-ce que le personnage est en train de glisser

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) // si la touche de glissement est enfoncée
        {
            if (!isSliding) // si le personnage n'est pas déjà en train de glisser
            {
                isSliding = true;
                slideTime = 0f;
            }
        }

        if (isSliding) // si le personnage est en train de glisser
        {
            slideTime += Time.deltaTime;
            if (slideTime > slideDuration) // si la durée de glissement est écoulée
            {
                isSliding = false;
            }
            else // sinon, faire glisser le personnage
            {
                controller.Move(transform.forward * slideSpeed * Time.deltaTime);
            }
        }
    }
}
