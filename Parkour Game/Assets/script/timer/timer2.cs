using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class timer2 : MonoBehaviour
{
    public Text timerText1;
    public Text bestTimerText1;
    private float startTime1;
    private bool finished = false;

    private float bestTime1 =10  ; // Variable pour stocker le meilleur temps
    void Start()
    {
        // Charger le meilleur temps précédent depuis les préférences de joueur
        startTime1 = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        bestTime1 = PlayerPrefs.GetFloat("BestTime2");
        if (finished)
            return;
        float t = Time.time - startTime1;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        timerText1.text = "Timer :" + minutes + ":" + seconds;
        bestTimerText1.text = "best time :" + bestTime1.ToString("F2") + " secondes";
        bestTimerText1.color = Color.green;
    }

    public void Finish()
    {
        if (!finished)
        {
            timerText1.color = Color.yellow;
            finished = true;
            // Comparaison avec le meilleur temps précédent
            float elapsedTime1 = Time.time - startTime1;
            if (bestTime1 == 0)
            {
                PlayerPrefs.DeleteKey("BestTime2");
                PlayerPrefs.SetFloat("BestTime2", elapsedTime1);
                PlayerPrefs.Save();
            }
            else if (elapsedTime1 < bestTime1)
            {
                bestTime1 = elapsedTime1;
                PlayerPrefs.SetFloat("BestTime2", bestTime1);
                PlayerPrefs.Save(); // Sauvegarder les préférences
                Debug.Log("Temps actuel : " + elapsedTime1 + " secondes");
                Debug.Log("Meilleur temps : " + bestTime1 + " secondes");
            }
        }
    }
}