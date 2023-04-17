
// using System;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using TMPro;

// public class GameManager : MonoBehaviour
// {
//     public playerMovement pm;
//     public GameObject escapeMenu, winScreen;
//     public GameObject[] speedDisplays;

//     //Timer
//     public float tSeconds, tMinutes;
//     public TextMeshProUGUI timerText, finalTimerText;
//     bool timerActive;

//     bool escapeMenuActive, allowEmSwitch;

//     //Sound
//     public AudioSource walkAudio, bgMusic;
//     public AudioSource landAudio, jumpAudio, wallRunAudio;
//     public AudioSource checkPointAudio;

//     private void Start()
//     {
//         timerActive = true;
//         allowEmSwitch = true;

//         pm = GameObject.Find("PlayerObj").GetComponent<playerMovement>();
//     }
//     void Update()
//     {
//         EscapeMenu();

//         if(timerActive)
//             Timer();
//     }
//     private void AudioManagement()
//     {
//         if (!bgMusic.isPlaying)
//             bgMusic.Play();

//         // if walking
//         walkAudio.Play();
//         //else
//         walkAudio.Stop();

//         landAudio.Play();
//         jumpAudio.Play();
//         wallRunAudio.Play();
//     }
//     private void Timer()
//     {
//         //Seconds
//         tSeconds += Time.deltaTime;

//         //Minutes
//         if (tSeconds >= 60)
//         {
//             tSeconds = 0;
//             tMinutes++;
//         }

//         //SetTimer
//         timerText.SetText(tMinutes + ":" + Math.Round(tSeconds, 1));
//     }
//     private void EscapeMenu()
//     {
//         if (Input.GetKeyDown(KeyCode.Escape) && !escapeMenuActive && allowEmSwitch)
//         {
//             allowEmSwitch = false;
//             escapeMenuActive = true;
//             escapeMenu.SetActive(true);

//             Cursor.lockState = CursorLockMode.None;
//             Cursor.visible = true;
//             pm.lockLook = true;

//             Invoke(nameof(ResetEmSwitch), 0.1f);
//         }
//         if (Input.GetKeyDown(KeyCode.Escape) && escapeMenuActive && allowEmSwitch)
//         {
//             allowEmSwitch = false;
//             escapeMenuActive = false;
//             escapeMenu.SetActive(false);

//             Cursor.lockState = CursorLockMode.Locked;
//             Cursor.visible = false;
//             pm.lockLook = false;

//             Invoke(nameof(ResetEmSwitch), 0.1f);
//         }
//     }
//     private void ResetEmSwitch()
//     {
//         allowEmSwitch = true;
//     }

//     public void LevelFinished()
//     {
//         timerActive = false;

//         winScreen.SetActive(true);
//         escapeMenu.SetActive(false);
//         allowEmSwitch = false;

//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible = true;
//         pm.lockLook = true;

//         finalTimerText.SetText("Time: " + timerText.text);
//     }

//     #region Buttons
//     public void GoToMainMenu()
//     {
//         SceneManager.LoadScene(0);
//     }
//     public void RestartScene()
//     {
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//     }

//     public bool speedDisplaysActive;
//     public void ToggleSpeedDisplay()
//     {
//         speedDisplaysActive = !speedDisplaysActive;

//         for (int i = 0; i < speedDisplays.Length; i++)
//         {
//             speedDisplays[i].SetActive(speedDisplaysActive);
//         }       
//     }

//     #endregion
// }
