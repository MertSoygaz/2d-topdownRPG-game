using UnityEngine;
using DG.Tweening; 

public class EnemyHealth : MonoBehaviour
{
    public int health = 1;
    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health == 1 && gameObject.CompareTag("StrongEnemy"))
        {

            GetComponent<EnemyFollow>().speed = 3f;                  // speed boost
          
            if (_spriteRenderer != null)
            {
                _spriteRenderer.DOColor(Color.red, 0.3f);            
            }
        }
        
    }
}
