using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        // HUD sahnesini additive olarak y�kle
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }
}
