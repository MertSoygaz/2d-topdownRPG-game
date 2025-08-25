using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject redHitEffect;
    [SerializeField] private GameObject blackHitEffect;
    
    [SerializeField] private AudioClip shootSfx;
    
    [SerializeField] private BasicSpawner spawner;
    [SerializeField] private RangeDetector rangeDetector;
    [SerializeField] private Animator animator;

    private static readonly int ShootTrigger = Animator.StringToHash("shoot");
    private bool _isShooting;
    private float _shootTimer;
    private GameObject _currentTarget;

    
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float arrowSpeed = 25f;
    [SerializeField] private int arrowCount = 1;
    
    public float  ShootInterval {get => shootInterval; set => shootInterval = value; }                      // properties 
    public float  ArrowSpeed {get => arrowSpeed; set => arrowSpeed = value; }
    public int  ArrowCount {get => arrowCount; set => arrowCount = value; }
    
    private void Update()
    {
        _shootTimer += Time.deltaTime;

        if (!_isShooting && _shootTimer >= shootInterval)
        {
            var targets = rangeDetector.GetNearestEnemies(transform.position, arrowCount);

            if (targets.Count > 0)
            {
                StartCoroutine(ShootWithAnimation(targets));
            }
        }
    }

 

    private IEnumerator ShootWithAnimation(List<GameObject> targets)
    {
        _isShooting = true;
        animator.SetTrigger(ShootTrigger);
        yield return new WaitForSeconds(0.4f);                                                             // Animation time

        foreach (var target in targets)
        {
            if (target is null) continue;

            Vector2 directionToEnemy = target.transform.position - transform.position;
            var shootDirection = directionToEnemy.normalized;
            var moveDir = PlayerMovement.LastMoveX;
            var shootDirX = directionToEnemy.x;
            
            var shouldFlip = (moveDir > 0 && shootDirX < 0) || (moveDir < 0 && shootDirX > 0);
            if (shouldFlip)
            {
                var scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }


            var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(shootSfx, transform.position);

            var arrowScale = arrow.transform.localScale;
            arrowScale.x = directionToEnemy.x >= 0 ? Mathf.Abs(arrowScale.x) : -Mathf.Abs(arrowScale.x);
            arrow.transform.localScale = arrowScale;

            StartCoroutine(MoveArrow(arrow, shootDirection, target));
        }

        _shootTimer = 0f;
        _isShooting = false;
    }

    private IEnumerator MoveArrow(GameObject arrow, Vector2 direction, GameObject target)
    {
        var timer = 0f;
        const float lifeTime = 5f;

        while (arrow is not null && timer < lifeTime)
        {
            if (target is null)
            {
                Destroy(arrow);
                yield break;
            }

            arrow.transform.Translate(direction * (arrowSpeed * Time.deltaTime));

            if (Vector2.Distance(arrow.transform.position, target.transform.position) < 0.5f)
            {
                var enemyHealth = target.GetComponent<EnemyHealth>();
                var enemy = target.GetComponent<Enemy.Enemy>();
                if (enemyHealth is not null)
                {
                    enemyHealth.TakeDamage(1);

                    if (enemyHealth.Health <= 0)
                    {
                        enemy.KillEnemy();

                        var effectToPlay = target.tag switch
                        {
                            "Enemy" => redHitEffect,
                            "StrongEnemy" => blackHitEffect,
                            _ => null
                        };

                        if (effectToPlay is not null)
                        {
                            var effect = Instantiate(effectToPlay, target.transform.position, Quaternion.identity);
                            Destroy(effect, 1.5f);
                        }

                        
                        var isStrong = target.CompareTag("StrongEnemy");
                        spawner.ReturnToPool(target, isStrong);
                    }
                }

                Destroy(arrow);
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (arrow is not null)
            Destroy(arrow);
    }
}
