using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject redHitEffect;
    [SerializeField] private GameObject blackHitEffect;
    [SerializeField] private AudioClip shootSfx;

    private float _shootTimer;
    [SerializeField] private BasicSpawner spawner;
    [SerializeField] private RangeDetector rangeDetector;
    private GameObject _currentTarget;
    
    [SerializeField] private Animator animator;
    private static readonly int ShootTrigger = Animator.StringToHash("shoot");
    private bool _isShooting;
    
    public float shootInterval = 2f;
    public float arrowSpeed = 25f;
    public int arrowCount = 1;


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

    IEnumerator ShootWithAnimation(List<GameObject> targets)
    {
        _isShooting = true;
        animator.SetTrigger(ShootTrigger);
        yield return new WaitForSeconds(0.4f);    // Animation time

        foreach (var target in targets)
        {
            if (target == null) continue;

            Vector2 directionToEnemy = target.transform.position - transform.position;
            Vector2 shootDirection = directionToEnemy.normalized;

            float moveDir = PlayerMovement.LastMoveX;
            float shootDirX = directionToEnemy.x;

            bool shouldFlip = (moveDir > 0 && shootDirX < 0) || (moveDir < 0 && shootDirX > 0);
            if (shouldFlip)
            {
                var scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }


            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(shootSfx, transform.position);

            var arrowScale = arrow.transform.localScale;
            arrowScale.x = directionToEnemy.x >= 0 ? Mathf.Abs(arrowScale.x) : -Mathf.Abs(arrowScale.x);
            arrow.transform.localScale = arrowScale;

            StartCoroutine(MoveArrow(arrow, shootDirection, target));
        }

        _shootTimer = 0f;
        _isShooting = false;
    }

    IEnumerator MoveArrow(GameObject arrow, Vector2 direction, GameObject target)
    {
        float timer = 0f;
        float lifeTime = 5f;

        while (arrow != null && timer < lifeTime)
        {
            if (target == null)
            {
                Destroy(arrow);
                yield break;
            }

            arrow.transform.Translate(direction * (arrowSpeed * Time.deltaTime));

            if (Vector2.Distance(arrow.transform.position, target.transform.position) < 0.5f)
            {
                EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(1);

                    if (enemyHealth.health <= 0)
                    {
                        GameObject effectToPlay = null;

                        var enemy = target.GetComponent<Enemy>();
                        enemy.KillEnemy();
                   
                        switch (target.tag)
                        {
                            case "Enemy":
                                effectToPlay = redHitEffect;
                                break;

                            case "StrongEnemy":
                                effectToPlay = blackHitEffect;
                                break;
                        }

                        if (effectToPlay != null)
                        {
                            GameObject effect = Instantiate(effectToPlay, target.transform.position, Quaternion.identity);
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

        if (arrow != null)
            Destroy(arrow);
    }
}
