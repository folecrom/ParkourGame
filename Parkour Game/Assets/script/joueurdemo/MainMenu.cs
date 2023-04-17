
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playScene, levelSelection;

    public void PlayButton()
    {
        playScene.SetActive(false);
        levelSelection.SetActive(true);
    }

    public void BackToMainScreen()
    {
        playScene.SetActive(true);
        levelSelection.SetActive(false);
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene("Level" + levelNumber);
    }
    public void LoadEndlessLevel()
    {
        SceneManager.LoadScene("EndlessMode");
    }
}
