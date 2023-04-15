using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Adam").SendMessage("Finish");
}
}
