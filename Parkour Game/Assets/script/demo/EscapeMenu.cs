
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    public GameObject escapeMenu;

    public bool active;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !active)
        {
            active = true;
            escapeMenu.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && active)
        {
            active = false;
            escapeMenu.SetActive(false);
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
