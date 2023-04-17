using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGo : MonoBehaviour
{
    public GameObject[] activate;
    public GameObject[] deActivate;

    public bool activateOnEnter, activateOnCollision;

    private void OnCollisionEnter(Collision collision)
    {
        if (activateOnCollision && collision.collider.CompareTag("Player"))
        {
            Run();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activateOnEnter && other.CompareTag("Player"))
        {
            Run();
        }
    }

    private void Run()
    {
        for (int i = 0; i < activate.Length; i++)
        {
            activate[i].SetActive(true);
        }
        for (int i = 0; i < deActivate.Length; i++)
        {
            deActivate[i].SetActive(false);
        }
    }
}
