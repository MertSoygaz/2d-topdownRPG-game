using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    public List<GameObject> enemiesInRange = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out _) && !enemiesInRange.Contains(collision.gameObject))
        {
         
            enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out _) && enemiesInRange.Contains(collision.gameObject))
        {
            enemiesInRange.Remove(collision.gameObject);
        }
    }

    public List<GameObject> GetNearestEnemies(Vector3 fromPosition, int count)
    {
        List<GameObject> validEnemies = enemiesInRange.Where(e => e != null).ToList();

        return validEnemies
            .OrderBy(e => Vector2.Distance(fromPosition, e.transform.position))
            .Take(count)
            .ToList();
    }


}
