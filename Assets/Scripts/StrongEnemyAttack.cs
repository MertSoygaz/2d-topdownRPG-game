using UnityEngine;

public class StrongEnemyAttack : MonoBehaviour
{
    public float attackRange = 1.75f;
    private GameObject player;
    private Animator animator;
    public AudioClip swordStabSFX;

    private bool hasHitThisAttack = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("StrongEnemyAttacking"))
            {
                animator.SetTrigger("strongEnemyAttack");
                hasHitThisAttack = false; 
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (hasHitThisAttack) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("StrongEnemyAttacking"))
            {
                Debug.Log("Hit");
                hasHitThisAttack = true;

                AudioListener listener = FindFirstObjectByType<AudioListener>();
                Vector3 audioPosition = listener != null ? listener.transform.position : Vector3.zero;
                AudioSource.PlayClipAtPoint(swordStabSFX, audioPosition);


                float damage = 0.1f;
                PlayerHealthBarRect.Instance.TakeDamage(damage);
            }
        }
    }
}
