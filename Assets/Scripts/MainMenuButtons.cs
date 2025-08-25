using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip buttonSfx;
     
    public void QuitGame()
    {
        source.PlayOneShot(buttonSfx);
        Application.Quit();
    }

    public void PlayGame()
    {
        source.PlayOneShot(buttonSfx);
        StartCoroutine(LoadAfterDelay(0.5f));
    }

    private static IEnumerator LoadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameplayScene");
    }


}
