using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float totalTime = 601f; 

    private float _elapsedTime;
    private bool _isPaused;

    private void Start()
    {
        _elapsedTime = 0f;
        _isPaused = false;
        UpdateTimerUI();
    }

    private void Update()
    {
        if (_isPaused || _elapsedTime >= totalTime) return;

        _elapsedTime += Time.deltaTime;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        var timeLeft = Mathf.Max(0f, totalTime - _elapsedTime);
        var minutes = Mathf.FloorToInt(timeLeft / 60f);
        var seconds = Mathf.FloorToInt(timeLeft % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}