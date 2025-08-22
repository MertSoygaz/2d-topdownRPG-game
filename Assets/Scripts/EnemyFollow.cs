using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 1.25f;
    private Transform _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (_player == null) return;

        Vector2 direction = (_player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * (speed * Time.deltaTime));

        if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }
}
