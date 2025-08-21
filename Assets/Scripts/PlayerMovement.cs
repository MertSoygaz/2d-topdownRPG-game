using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    public Rigidbody2D rb;
    public Animator animator;

    public static float LastMoveX { get; private set; }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Hareket yönünü güncelle
        if (horizontal != 0)
        {
            LastMoveX = horizontal;

            // Sprite yönünü güncelle
            Vector3 scale = transform.localScale;
            scale.x = horizontal > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        // Animator parametreleri
        animator.SetFloat("horizontal", Mathf.Abs(horizontal));
        animator.SetFloat("vertical", Mathf.Abs(vertical));

        // Hareket
        rb.linearVelocity = new Vector2(horizontal, vertical) * speed;
    }
}
