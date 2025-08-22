using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        // Load HUD Scene as Additive
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }
}
