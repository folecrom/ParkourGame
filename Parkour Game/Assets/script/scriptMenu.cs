using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PanelType{
    Name, 
    Main,
    Settings
}
public class scriptMenu : MonoBehaviour
{
    private GameManager manager;

    private void Start()
    {
        manager =  GameManager.instance;
    }

    public void OpenPanel(){
         
    }
    public void ChangeScene(string _sceneName)
    {
        manager.ChangeScene(_sceneName);
    }
    public void QuitGame()
    {
        manager.QuitGame();
    }
};
