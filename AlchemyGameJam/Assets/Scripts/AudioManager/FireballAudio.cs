using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireballAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] fireballClips;
    [SerializeField] private float pitchVariation = 0.1f;
    [SerializeField] private float volume = 1f;

    private AudioSource audioSource;
    private int lastIndex = -1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (fireballClips.Length == 0) return;

        int index;
        do
        {
            index = Random.Range(0, fireballClips.Length);
        }
        while (index == lastIndex);

        lastIndex = index;

        audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(fireballClips[index], volume);
    }
}