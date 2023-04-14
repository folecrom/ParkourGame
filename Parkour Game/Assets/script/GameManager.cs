using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
            instance = this;
            DontDestroyOnLoad(gameObject);
    }
    
    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
};

