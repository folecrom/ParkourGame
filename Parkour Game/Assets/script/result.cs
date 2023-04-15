using UnityEngine;
using UnityEngine.UI;

public class result : MonoBehaviour
{
    public Text resultText;
    private float bestTime;

    private void Start()
    {
        // Charger la valeur de bestTime depuis les préférences de joueur
        bestTime = PlayerPrefs.GetFloat("BestTime");
        resultText.text = "best time :" + bestTime.ToString("F2") + " secondes";
        resultText.color = Color.green;
    }
}