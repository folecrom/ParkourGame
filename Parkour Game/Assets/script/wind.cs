using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class wind : MonoBehaviour
{
    private async void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Adam").SendMessage("Finish");
        await Task.Delay(3000); // 3000 ms = 3 secondes

        // Charger la scène "MENU"
        SceneManager.LoadScene("MENU");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
