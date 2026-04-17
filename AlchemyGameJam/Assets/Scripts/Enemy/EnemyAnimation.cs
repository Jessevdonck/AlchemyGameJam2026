using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sprite;
    
    private bool isDead = false;

    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    
    private static readonly int DieTrigger = Animator.StringToHash("Die");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    public void PlayDeath()
    {
        if (animator != null)
        {
            isDead = true;

            // Reset movement zodat blend tree niet blijft pushen
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);

            animator.SetTrigger(DieTrigger);
        }
    }
    public void UpdateAnimation(Vector2 direction)
    {
        if (animator == null || isDead) return;

        float x = Mathf.Abs(direction.x);
        float y = direction.y;

        animator.SetFloat(MoveX, x);
        animator.SetFloat(MoveY, y);

        if (sprite != null)
        {
            if (direction.x < -0.1f)
                sprite.flipX = true;
            else if (direction.x > 0.1f)
                sprite.flipX = false;
        }
    }
}