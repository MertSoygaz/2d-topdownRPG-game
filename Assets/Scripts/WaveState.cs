using UnityEngine;
using TMPro;
using System.Collections;

public class WaveState : MonoBehaviour
{
    public static WaveState Instance;
    public TMP_Text waveText;
    public float displayDuration = 3f;
    public float fadeDuration = 0.75f;
    public AudioClip whooshSFX;

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

        AudioListener listener = FindFirstObjectByType<AudioListener>();
        Vector3 audioPosition = listener != null ? listener.transform.position : Vector3.zero;
        AudioSource.PlayClipAtPoint(whooshSFX,audioPosition);

        Color startColor = waveText.color;
        startColor.a = 1f;
        waveText.color = startColor;

        yield return new WaitForSeconds(displayDuration);

        // Fade out
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            Color c = waveText.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            waveText.color = c;

            yield return null;
        }

        waveText.enabled = false;
    }
}
