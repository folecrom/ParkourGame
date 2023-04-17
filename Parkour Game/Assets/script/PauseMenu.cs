using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool IsPaused;
    public GameObject pauseMenu;
    private GameManager manager;
    void Start()
    {
        manager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
        }
    }
    public void Resume()
    {
        IsPaused = !IsPaused;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MENU");
    }
    public void QuitGame()
    {
        manager.QuitGame();
    }
}
