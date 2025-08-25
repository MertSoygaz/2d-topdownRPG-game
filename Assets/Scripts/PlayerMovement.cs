using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    private static readonly int Horizontal = Animator.StringToHash("horizontal");
    private static readonly int Vertical = Animator.StringToHash("vertical");

    public static float LastMoveX { get; private set; }

    private void FixedUpdate()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        var input = new Vector2(horizontal, vertical).normalized; // normalize et

        if (horizontal != 0)
        {
            LastMoveX = horizontal;

            var scale = transform.localScale;
            scale.x = horizontal > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        animator.SetFloat(Horizontal, Mathf.Abs(horizontal));
        animator.SetFloat(Vertical, Mathf.Abs(vertical));

        rb.linearVelocity = input * speed; 
    }
}