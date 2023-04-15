using UnityEngine;
using UnityEngine.UI;

public class result : MonoBehaviour
{
    public Text resultText;
    private float bestTime = 0;

    private void Start()
    {
        // Charger la valeur de bestTime depuis les préférences de joueur
        bestTime = PlayerPrefs.GetFloat("BestTime");
        resultText.text = "best time :" + bestTime.ToString("F2") + " secondes";
        resultText.color = Color.green;
    }

    private void Update()
    {
        // Utiliser la valeur de bestTime comme vous le souhaitez
        Debug.Log(bestTime + " secondes");
    }
}