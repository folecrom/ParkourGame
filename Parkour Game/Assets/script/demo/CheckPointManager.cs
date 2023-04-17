// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CheckPointManager : MonoBehaviour
// {
//     public Transform[] checkPoints;

//     public Transform player;
//     Vector3 spawnPoint;
//     public GameManager gm;
//     public Transform checkPointMarker;
//     public LayerMask whatIsPlayer;

//     public Vector3 cpSize;
//     public int currCp = 0;
//     public float minY;

//     bool waiting;

//     private void Start()
//     {
//         player = GameObject.Find("PlayerObj").transform;
//         spawnPoint = player.position;
//         checkPointMarker.position = checkPoints[currCp].position;
//     }
//     private void Update()
//     {
//         if (player.position.y < minY) GoToLastCheckPoint();

//         //Check point reached
//         if(Physics.CheckBox(checkPoints[currCp].position, cpSize, Quaternion.identity, whatIsPlayer) && !waiting)
//         {
//             waiting = true;

//             NextCheckPoint();
//             Invoke(nameof(Wait), 1f);
//         }
//     }
//     private void Wait()
//     {
//         waiting = false;
//     }
//     private void NextCheckPoint()
//     {
//         currCp++;
//         //Check if level finished
//         if (currCp == checkPoints.Length)
//         {
//             gm.LevelFinished();
//             checkPointMarker.gameObject.SetActive(false);
//             return;
//         }

//         checkPointMarker.position = checkPoints[currCp].position;
//     }
//     public void GoToLastCheckPoint()
//     {
//         if (currCp - 1 >= 0)
//             player.position = checkPoints[currCp - 1].position;
//         else
//             player.position = spawnPoint;
//     }
// }
