using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.6f;
    private GameObject _player;
    private Animator _animator;
    [SerializeField] private AudioClip swordStabSfx;


    private bool _hasHitThisAttack = false;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttacking"))
            {
                _animator.SetTrigger("enemyAttack");
                _hasHitThisAttack = false;  
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_hasHitThisAttack) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttacking"))
            {
                Debug.Log("Hit");
                _hasHitThisAttack = true;

                AudioListener listener = FindFirstObjectByType<AudioListener>();
                Vector3 audioPosition = listener != null ? listener.transform.position : Vector3.zero;
                AudioSource.PlayClipAtPoint(swordStabSfx, audioPosition);

                float damage = 0.05f;
                PlayerHealthBarRect.Instance.TakeDamage(damage);
            }
        }
    }
}
