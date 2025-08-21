using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        // HUD sahnesini additive olarak yükle
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }
}
