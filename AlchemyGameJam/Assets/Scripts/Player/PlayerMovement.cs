using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    [Header("Dash")]
    public float dashForce = 15f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private Camera cam;
    private InputReader input;

    private bool isDashing;
    private bool canDash = true;

    public void Initialize(InputReader inputReader)
    {
        input = inputReader;
        input.OnDash += TryDash;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = input.MoveInput * speed;
        }
    }

    private void Update()
    {
        Vector2 move = input.MoveInput;
        float speedValue = move.magnitude;
        
        animator.SetFloat("Speed", speedValue, 0.1f, Time.deltaTime);
        
        Aim();
    }

    void Aim()
    {
        Vector2 worldPos = cam.ScreenToWorldPoint(input.MousePosition);
        Vector2 direction = worldPos - rb.position;
        Vector2 aimDir = (worldPos - rb.position).normalized;

        animator.SetFloat("AimX", aimDir.x);
        animator.SetFloat("AimY", aimDir.y);
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    void TryDash()
    {
        if (canDash && input.MoveInput != Vector2.zero)
        {
            StartCoroutine(Dash());
        }
    }

    System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        Vector2 dashDir = input.MoveInput.normalized;
        rb.linearVelocity = dashDir * dashForce;

        yield return new WaitForSeconds(dashTime);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}