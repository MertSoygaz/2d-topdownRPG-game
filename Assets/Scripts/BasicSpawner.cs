using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Wave;

public class BasicSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject strongEnemyPrefab;

    [Header("Pool Settings")] 
    [SerializeField] private int enemyPoolSize = 70;
    [SerializeField] private int strongEnemyPoolSize = 40;
    [SerializeField] private float spawnDistance = 30f;
    
    [SerializeField] private WaveData waveData;
    
    private readonly Queue<GameObject> _enemyPool = new();
    private readonly Queue<GameObject> _strongEnemyPool = new();

    
    

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
            obj.name = $"Enemy_{i}";
            obj.SetActive(false);
            _enemyPool.Enqueue(obj);                                        // Adds 70 (enemyPoolSize) enemy to pool
        }

        for (var i = 0; i < strongEnemyPoolSize; i++)
        {
            var obj = Instantiate(strongEnemyPrefab);
            obj.name = $"Strong Enemy_{i}";
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

        for (var i = 0; i < waveData.Waves.Count; i++)
        {
            _currentWave = i + 1;
            Debug.Log("Wave " + _currentWave + " started");
            WaveState.Instance?.ShowWave(_currentWave);
            
            if(cam==null) yield break;
            var center = cam.transform.position;
            var enemyCount = waveData.Waves[i].enemyCount;
            var strongEnemyCount = waveData.Waves[i].strongEnemyCount;

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
            while (timer < 60f)
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
