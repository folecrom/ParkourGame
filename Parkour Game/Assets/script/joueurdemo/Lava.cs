using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("LavaWorking");

            if (GameObject.Find("EndlessLevelManager"))
            {
                GameObject.Find("EndlessLevelManager").GetComponent<EndlessLevelManager>().gameOver = true;
                Debug.Log("GameOver");
                return;
            }

            if (GameObject.Find("GameManager"))
                GameObject.Find("CheckPointManager").GetComponent<CheckPointManager>().GoToLastCheckPoint();
        }
    }
}
