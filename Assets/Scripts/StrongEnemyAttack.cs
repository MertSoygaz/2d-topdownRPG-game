using UnityEngine;

public class StrongEnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.75f;
    [SerializeField] private AudioClip swordStabSfx;
    
    private GameObject _player;
    private Animator _animator;
    private static readonly int Attack = Animator.StringToHash("strong Enemy Attack");
    private bool _hasHitThisAttack;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_player is null) return;

        var distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("StrongEnemyAttacking"))
            {
                _animator.SetTrigger(Attack);
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

                var listener = FindFirstObjectByType<AudioListener>();
                var audioPosition = listener != null ? listener.transform.position : Vector3.zero;
                AudioSource.PlayClipAtPoint(swordStabSfx, audioPosition);


                const float damage = 0.1f;
                PlayerHealthBarRect.Instance.TakeDamage(damage);
            }
        }
    }
}
