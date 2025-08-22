using UnityEngine;

public class StrongEnemyAttack : MonoBehaviour
{
    public float attackRange = 1.75f;
    private GameObject _player;
    private Animator _animator;
    [SerializeField] private AudioClip swordStabSfx;

    private bool _hasHitThisAttack;

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
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("StrongEnemyAttacking"))
            {
                _animator.SetTrigger("strongEnemyAttack");
                _hasHitThisAttack = false; 
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_hasHitThisAttack) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("StrongEnemyAttacking"))
            {
                Debug.Log("Hit");
                _hasHitThisAttack = true;

                AudioListener listener = FindFirstObjectByType<AudioListener>();
                Vector3 audioPosition = listener != null ? listener.transform.position : Vector3.zero;
                AudioSource.PlayClipAtPoint(swordStabSfx, audioPosition);


                float damage = 0.1f;
                PlayerHealthBarRect.Instance.TakeDamage(damage);
            }
        }
    }
}
