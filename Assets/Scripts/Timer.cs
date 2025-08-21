using System.Collections;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    private float totalTime = 601f;
    private float elapsedTime = 0f;
    private bool timerRunning = false;
    private bool isPaused = false; 

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (timerRunning && !isPaused)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
            if (elapsedTime >= totalTime)
            {
                timerRunning = false;
            }
        }
    }

    void StartTimer()
    {
        timerRunning = true;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        float timeLeft = totalTime - elapsedTime;
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }
}
