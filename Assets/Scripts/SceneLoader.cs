using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        // Load HUD Scene as Additive
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }
}
