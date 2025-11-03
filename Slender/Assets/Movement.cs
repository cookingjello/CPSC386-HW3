using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 0.5f;
    private Rigidbody2D rb;
    private Vector2 input;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Get movement input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();

        // --- Animator parameters ---
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
        animator.SetBool("isWalking", input.magnitude > 0);

        // Record last direction moved (used for idle facing)
        if (input.magnitude > 0)
        {
            animator.SetFloat("LastInputX", input.x);
            animator.SetFloat("LastInputY", input.y);
        }

        // Flip sprite only when moving left
        if (input.x < 0)
            spriteRenderer.flipX = true;
        else if (input.x > 0)
            spriteRenderer.flipX = false;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * speed;
    }
}
