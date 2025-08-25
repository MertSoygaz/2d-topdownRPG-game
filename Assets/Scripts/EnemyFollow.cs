using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float speed = 1.25f;
    public float Speed { get => speed; set => speed = value;}
    
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }      

    private void Update()
    {
        if (_player is null) return;
        Vector2 direction = (_player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * (speed * Time.deltaTime));

        transform.localScale = direction.x switch
        {
            < 0 => new Vector3(-1, 1, 1),
            > 0 => new Vector3(1, 1, 1),
            _ => transform.localScale
        };
    }
}
