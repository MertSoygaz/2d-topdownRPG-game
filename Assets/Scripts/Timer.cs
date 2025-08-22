using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    private const float TotalTime = 601f;
    private float _elapsedTime;
    private bool _timerRunning;
    private bool _isPaused;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (_timerRunning && !_isPaused)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerUI();
            if (_elapsedTime >= TotalTime)
            {
                _timerRunning = false;
            }
        }
    }

    void StartTimer()
    {
        _timerRunning = true;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        var timeLeft = TotalTime - _elapsedTime;
        var minutes = Mathf.FloorToInt(timeLeft / 60);
        var seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";                        // string.Format("{0:00}:{1:00}", minutes, seconds);


    }

    public void PauseTimer()
    {
        _isPaused = true;
    }

    public void ResumeTimer()
    {
        _isPaused = false;
    }
}