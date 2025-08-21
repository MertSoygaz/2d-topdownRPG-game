using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public AudioSource source;
    public AudioClip buttonSFX;
     
    public void QuitGame()
    {
        source.PlayOneShot(buttonSFX);
        Application.Quit();
    }

    public void PlayGame()
    {
        source.PlayOneShot(buttonSFX);
        StartCoroutine(LoadAfterDelay(0.5f));
    }

    IEnumerator LoadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameplayScene");
    }


}
