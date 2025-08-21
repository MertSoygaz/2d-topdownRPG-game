using UnityEngine;
using DG.Tweening; 

public class EnemyHealth : MonoBehaviour
{
    public int health = 1;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health == 1 && gameObject.CompareTag("StrongEnemy"))
        {

            GetComponent<EnemyFollow>().speed = 3f;                  // speed boost
          
            if (spriteRenderer != null)
            {
                spriteRenderer.DOColor(Color.red, 0.3f);            
            }
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
