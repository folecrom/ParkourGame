using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timera : MonoBehaviour
{
    public Text timerText;
    public Text bestTimerText;
    private float startTime;
    private bool finished = false;

    private float bestTime; // Variable pour stocker le meilleur temps
    void Start()
    {
        // Charger le meilleur temps précédent depuis les préférences de joueur
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        bestTime = PlayerPrefs.GetFloat("BestTime");
        if (finished)
            return;
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        timerText.text = "Timer :"+minutes + ":" + seconds;
        bestTimerText.text = "best time :" + bestTime.ToString("F2") + " secondes";
        bestTimerText.color = Color.green;
    }

    public void Finish()
    {
        if (!finished)
        {
            timerText.color = Color.yellow;
            finished = true;

            // Comparaison avec le meilleur temps précédent
            float elapsedTime = Time.time - startTime;
            if (bestTime == 0)
            {
                PlayerPrefs.SetFloat("BestTime", elapsedTime);
                PlayerPrefs.Save();
            }
            else if (elapsedTime < bestTime)
            {
                bestTime = elapsedTime;
                PlayerPrefs.SetFloat("BestTime", bestTime);
                PlayerPrefs.Save(); // Sauvegarder les préférences
                Debug.Log("Temps actuel : " + elapsedTime + " secondes");
                Debug.Log("Meilleur temps : " + bestTime + " secondes");
            }
        }
    }
}
