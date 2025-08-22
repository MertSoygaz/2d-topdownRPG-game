using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    private readonly List<GameObject> _enemiesInRange = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out _) && !_enemiesInRange.Contains(collision.gameObject))
        {
         
            _enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out _) && _enemiesInRange.Contains(collision.gameObject))
        {
            _enemiesInRange.Remove(collision.gameObject);
        }
    }

    public List<GameObject> GetNearestEnemies(Vector3 fromPosition, int count)
    {
        List<GameObject> validEnemies = _enemiesInRange.Where(e => e != null).ToList();

        return validEnemies
            .OrderBy(e => Vector2.Distance(fromPosition, e.transform.position))
            .Take(count)
            .ToList();
    }


}
