using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject strongEnemyPrefab;

    [Header("Pool Settings")] 
    [SerializeField] private int enemyPoolSize = 70;
    [SerializeField] private int strongEnemyPoolSize = 40;
    [SerializeField] private float spawnDistance = 30f;

    private readonly Queue<GameObject> _enemyPool = new();
    private readonly Queue<GameObject> _strongEnemyPool = new();

    private readonly (int enemyCount, int strongEnemyCount)[] _waveData = {
        (12, 4),
        (15, 5),
        (17, 6),
        (20, 8),
        (25, 10),
        (30, 12),
        (40, 15), 
        (45, 20),
        (55, 25),
        (60, 30)
    };

    private int _currentWave;
    private readonly List<GameObject> _aliveEnemies = new();

    private void Start()
    {
        InitPools();
        StartCoroutine(DelayedWaveStart());
    }

    private void InitPools()
    {
        for (var i = 0; i < enemyPoolSize; i++)
        {
            var obj = Instantiate(enemyPrefab);
            obj.name = $"enemy_{i}";
            obj.SetActive(false);
            _enemyPool.Enqueue(obj);                                        // Adds 70 (enemyPoolSize) enemy to pool
        }

        for (var i = 0; i < strongEnemyPoolSize; i++)
        {
            var obj = Instantiate(strongEnemyPrefab);
            obj.name = $"strongenemy_{i}";
            obj.SetActive(false);
            _strongEnemyPool.Enqueue(obj);                                 // Adds 40 (strongEnemyPoolSize) strong enemy to pool
        }
    }

    private IEnumerator DelayedWaveStart()
    {
        yield return new WaitForSeconds(1f);
        WaveState.Instance?.ShowWave(1);                // null control
        
        //StartCoroutine(SpawnWaves());            
        StartCoroutine(nameof(SpawnWaves));           
    }

    private IEnumerator SpawnWaves()
    {
        var cam = Camera.main;

        for (var i = 0; i < _waveData.Length; i++)
        {
            _currentWave = i + 1;
            Debug.Log("Wave " + _currentWave + " started");
            WaveState.Instance?.ShowWave(_currentWave);
            
            if(cam==null) yield break;
            var center = cam.transform.position;
            var enemyCount = _waveData[i].enemyCount;
            var strongEnemyCount = _waveData[i].strongEnemyCount;

            _aliveEnemies.Clear();

            for (var j = 0; j < enemyCount; j++)
            {
                var spawnPos = GetRandomSpawnPosition(center);
                var enemy = GetFromPool(_enemyPool, enemyPrefab);
                enemy.transform.position = spawnPos;
                enemy.SetActive(true);
                _aliveEnemies.Add(enemy);
            }

            for (var j = 0; j < strongEnemyCount; j++)
            {
                var spawnPos = GetRandomSpawnPosition(center);
                var strongEnemy = GetFromPool(_strongEnemyPool, strongEnemyPrefab);
                strongEnemy.transform.position = spawnPos;
                strongEnemy.SetActive(true);
                _aliveEnemies.Add(strongEnemy);
            }

            var timer = 0f;
            while (timer < 30f)
            {
                _aliveEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);

                if (_aliveEnemies.Count == 0)
                {
                    Debug.Log("Wave " + _currentWave + " cleared early!");
                    yield return new WaitForSeconds(2f);
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    private GameObject GetFromPool(Queue<GameObject> pool, GameObject prefab)
    {
        if (pool.Count > 0)
        {
            var obj = pool.Dequeue();
            return obj;
        }
        else
        {
            var obj = Instantiate(prefab);
            return obj;
        }
    }

    private Vector3 GetRandomSpawnPosition(Vector3 center)
    {
        var angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        var offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * spawnDistance;
        var spawnPos = center + offset;
        spawnPos.z = 0f;
        return spawnPos;
    }

    public void ReturnToPool(GameObject obj, bool isStrong)
    {
        //Debug.Log("Object returned to pool");
        obj.SetActive(false);
        if (isStrong)
            _strongEnemyPool.Enqueue(obj);
        else
            _enemyPool.Enqueue(obj);
    }
}
