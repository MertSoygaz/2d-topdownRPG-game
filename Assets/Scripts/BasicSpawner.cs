using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject strongEnemyPrefab;
    public float spawnDistance = 12f;

    private (int enemyCount, int strongEnemyCount)[] waveData = new (int, int)[]
    {
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

    public int CurrentWave { get; private set; } = 0;

    // Her wave’de spawn edilen düşmanlar burada tutulacak
    private List<GameObject> aliveEnemies = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(DelayedWaveStart());
    }

    private IEnumerator DelayedWaveStart()
    {
        yield return new WaitForSeconds(1f); 
        WaveState.Instance?.ShowWave(1);
        StartCoroutine(SpawnWaves());
    }

    public IEnumerator SpawnWaves()
    {
        Camera cam = Camera.main;

        for (int i = 0; i < waveData.Length; i++)
        {
            CurrentWave = i + 1;
            Debug.Log("Wave " + CurrentWave + " started");

            // Wave UI göster
            WaveState.Instance?.ShowWave(CurrentWave);

            Vector3 center = cam.transform.position;
            int enemyCount = waveData[i].enemyCount;
            int strongEnemyCount = waveData[i].strongEnemyCount;

            aliveEnemies.Clear();

            for (int j = 0; j < enemyCount; j++)
            {
                Vector3 spawnPos = GetRandomSpawnPosition(center);
                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                aliveEnemies.Add(enemy);
            }

            for (int j = 0; j < strongEnemyCount; j++)
            {
                Vector3 spawnPos = GetRandomSpawnPosition(center);
                GameObject strongEnemy = Instantiate(strongEnemyPrefab, spawnPos, Quaternion.identity);
                aliveEnemies.Add(strongEnemy);
            }

            // 60 saniye maksimum bekleme
            float timer = 0f;
            while (timer < 60f)
            {
                // Listeden null olanları (ölmüş olan düşmanları) temizle
                aliveEnemies.RemoveAll(e => e == null);

                // Eğer hiç düşman kalmadıysa -> 2 saniye bekle, sonraki wave
                if (aliveEnemies.Count == 0)
                {
                    Debug.Log("Wave " + CurrentWave + " cleared early!");
                    yield return new WaitForSeconds(2f);
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    private Vector3 GetRandomSpawnPosition(Vector3 center)
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * spawnDistance;
        Vector3 spawnPos = center + offset;
        spawnPos.z = 0f;
        return spawnPos;
    }
}
