using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening; 

public class WaveState : MonoBehaviour
{
    public static WaveState Instance;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float fadeDuration = 0.75f;
    [SerializeField] private AudioClip whooshSfx;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        waveText.text = "";
    }

    public void ShowWave(int waveNumber)
    {
        StopAllCoroutines();
        StartCoroutine(ShowWaveRoutine(waveNumber));
    }

    private IEnumerator ShowWaveRoutine(int waveNumber)
    {
        waveText.text = "Wave " + waveNumber;
        waveText.enabled = true;

        var listener = FindFirstObjectByType<AudioListener>();
        var audioPosition = listener is not null ? listener.transform.position : Vector3.zero;
        AudioSource.PlayClipAtPoint(whooshSfx, audioPosition);

        waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, 1f);

        yield return new WaitForSeconds(displayDuration);

        // DOTween fade out
        waveText.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            waveText.enabled = false;
        });
    }
}