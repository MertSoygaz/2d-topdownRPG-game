using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarRect : MonoBehaviour
{
    public static PlayerHealthBarRect Instance;

    public Image fillImage; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TakeDamage(float damageFraction)
    {
        fillImage.fillAmount = Mathf.Max(0f, fillImage.fillAmount - damageFraction);
    }
}