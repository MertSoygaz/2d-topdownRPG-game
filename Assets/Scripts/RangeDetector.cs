using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    private readonly List<GameObject> _enemiesInRange = new();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy.Enemy>(out _) && !_enemiesInRange.Contains(collision.gameObject))
        {
         
            _enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy.Enemy>(out _) && _enemiesInRange.Contains(collision.gameObject))
        {
            _enemiesInRange.Remove(collision.gameObject);
        }
    }

    public List<GameObject> GetNearestEnemies(Vector3 fromPosition, int count)
    {
        var validEnemies = _enemiesInRange.Where(e => e is not null).ToList();

        return validEnemies
            .OrderBy(e => Vector2.Distance(fromPosition, e.transform.position))
            .Take(count)
            .ToList();
    }
}
