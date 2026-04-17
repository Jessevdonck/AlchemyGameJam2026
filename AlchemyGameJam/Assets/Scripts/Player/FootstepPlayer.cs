using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepPlayer : MonoBehaviour
{
    [Header("Footsteps")]
    [SerializeField] private AudioClip[] footstepClips;

    [Header("Settings")]
    [SerializeField] private float stepInterval = 0.3f;
    [SerializeField] private float pitchVariation = 0.1f;
    [SerializeField] private float volume = 0.1f;

    private AudioSource audioSource;
    private float stepTimer;

    private PlayerMovement movement;

    private int lastIndex = -1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (movement == null) return;

        if (movement.IsDashing())
            return;

        Vector2 input = movement.GetMoveInput();

        if (input.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep(input.magnitude);
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstep(float moveAmount)
    {
        if (footstepClips.Length == 0) return;

        int index;
        do
        {
            index = Random.Range(0, footstepClips.Length);
        }
        while (index == lastIndex);

        lastIndex = index;

        AudioClip clip = footstepClips[index];

        audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(clip, volume);

        stepTimer = stepInterval / Mathf.Max(moveAmount, 0.1f);
    }
}