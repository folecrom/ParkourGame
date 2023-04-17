using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public bool upward, forward;
    public bool usePlayersDirection;

    public bool preventPlayerDrag;
    public float preventDragTime;

    public float upwardForce, forwardForce;

    public Transform orientation;

    void Start()
    {
        orientation = GameObject.Find("Orientation").transform;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (forward)
            {
                if (usePlayersDirection)
                    collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.transform.forward * forwardForce, ForceMode.Impulse);
                else
                    collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * forwardForce, ForceMode.Impulse);
            }

            if (upward)
            {
                if (usePlayersDirection)
                    collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.transform.up * upwardForce, ForceMode.Impulse);
                else
                    collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * upwardForce, ForceMode.Impulse);
            }

            if (preventPlayerDrag)
                orientation.GetComponentInParent<playerMovement>().PreventDrag(preventDragTime);
        }
    }
}
