using UnityEngine;
using DG.Tweening;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private EnemyFollow enemyFollow;
    [SerializeField] private int health;

    public int Health
    {
        get => health;
        set => health = value;
    }

    private void Awake()
    {
        health = CompareTag("StrongEnemy") ? 2 : 1;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health == 1 && CompareTag("StrongEnemy"))
        {
            enemyFollow.Speed = 3f;
            spriteRenderer.DOColor(Color.red, 0.3f);
        }
    }
}