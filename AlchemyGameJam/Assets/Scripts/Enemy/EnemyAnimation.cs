using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sprite;

    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void UpdateAnimation(Vector2 direction)
    {
        if (animator == null) return;

        float x = Mathf.Abs(direction.x);
        float y = direction.y;

        animator.SetFloat(MoveX, x);
        animator.SetFloat(MoveY, y);

        // Flip voor links
        if (sprite != null)
        {
            if (direction.x < -0.1f)
                sprite.flipX = true;
            else if (direction.x > 0.1f)
                sprite.flipX = false;
        }
    }
}