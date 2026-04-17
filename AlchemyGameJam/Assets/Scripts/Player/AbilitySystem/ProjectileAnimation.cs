using UnityEngine;

public class ProjectileAnimation : MonoBehaviour
{
    private Animator animator;

    private static readonly int HitTrigger = Animator.StringToHash("Hit");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayHit()
    {
        if (animator != null)
            animator.SetTrigger(HitTrigger);
    }
}