using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slidingmovement : MonoBehaviour
{
    [Header("Refenrence")]

    [Header("Sliding")]
    private float maxSlideTime ;
    private float forceslide;
    private float slideTime;

    public void Startslide(){
        if (slideTime > 0)
    {
        slideTime -= Time.deltaTime;
        if (slideTime <= 0)
        {
            slideTime = 0;
        }
    }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Startslide();
        }
    }

}
