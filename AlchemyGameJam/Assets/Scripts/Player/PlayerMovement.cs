using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    [Header("Dash")]
    public float dashForce = 15f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    [Header("Audio")]
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioSource audioSource;

    private Rigidbody2D rb;
    private Animator animator;
    private Camera cam;
    private InputReader input;
    private Collider2D col;

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
        col = GetComponent<Collider2D>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
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

        animator.SetFloat("MoveX", move.x);
        animator.SetFloat("MoveY", move.y);
        animator.SetFloat("Speed", speedValue, 0.1f, Time.deltaTime);

        Aim();
    }

    void Aim()
    {
        Vector2 worldPos = cam.ScreenToWorldPoint(input.MousePosition);
        Vector2 direction = worldPos - rb.position;
        Vector2 aimDir = direction.normalized;

        animator.SetFloat("AimX", aimDir.x);
        animator.SetFloat("AimY", aimDir.y);
    }

    void TryDash()
    {
        if (canDash && input.MoveInput != Vector2.zero)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        Vector2 dashDir = input.MoveInput.normalized;

        if (dashClip != null && audioSource != null)
        {
            audioSource.pitch = 1f + Random.Range(-0.1f, 0.1f);
            audioSource.PlayOneShot(dashClip, 0.1f);
        }

        rb.linearVelocity = dashDir * dashForce;

        yield return new WaitForSeconds(dashTime);
        
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public Vector2 GetMoveInput()
    {
        return input != null ? input.MoveInput : Vector2.zero;
    }

    public bool IsDashing()
    {
        return isDashing;
    }
}