using UnityEngine;
using UnityEngine.UI;

public class result : MonoBehaviour
{
    public Text resultText;
    private float bestTime;
    public Text resultlevel2;
    private float bestTime2;

    private void Start()
    {
        // Charger la valeur de bestTime depuis les préférences de joueur
        bestTime = PlayerPrefs.GetFloat("BestTime");
        resultText.text = "best time :" + bestTime.ToString("F2") + " secondes";
        resultText.color = Color.green;
        bestTime2 = PlayerPrefs.GetFloat("BestTime2");
        resultlevel2.text = "best time :" + bestTime2.ToString("F2") + " secondes";
        resultlevel2.color = Color.green;
    }
}