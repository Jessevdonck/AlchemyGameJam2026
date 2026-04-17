using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Header("Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Music")]
        [SerializeField] private List<AudioClip> menuMusic;
        [SerializeField] private List<AudioClip> gameMusic;

        private List<AudioClip> currentPlaylist;
        private AudioClip lastTrack;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // -------- MUSIC --------

        public void PlayMenuMusic()
        {
            if (currentPlaylist == menuMusic) return;

            currentPlaylist = menuMusic;
            PlayRandomTrack();
        }

        public void PlayGameMusic()
        {
            if (currentPlaylist == gameMusic) return;

            currentPlaylist = gameMusic;
            PlayRandomTrack();
        }

        void PlayRandomTrack()
        {
            if (currentPlaylist == null || currentPlaylist.Count == 0) return;

            AudioClip next;

            do
            {
                next = currentPlaylist[Random.Range(0, currentPlaylist.Count)];
            }
            while (next == lastTrack && currentPlaylist.Count > 1);

            lastTrack = next;

            StopAllCoroutines();
            StartCoroutine(PlayTrackRoutine(next));
        }

        IEnumerator PlayTrackRoutine(AudioClip clip)
        {
            yield return StartCoroutine(FadeMusic(clip));

            yield return new WaitForSeconds(clip.length);

            PlayRandomTrack();
        }

        IEnumerator FadeMusic(AudioClip newClip)
        {
            float duration = 0.5f;
            float t = 0;

            float startVolume = musicSource.volume;

            // fade out
            while (t < duration)
            {
                t += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
                yield return null;
            }

            musicSource.clip = newClip;
            musicSource.Play();

            t = 0;

            // fade in
            while (t < duration)
            {
                t += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(0f, startVolume, t / duration);
                yield return null;
            }
        }

        // -------- SFX --------

        public void PlaySFX(AudioClip clip)
        {
            sfxSource.pitch = Random.Range(0.95f, 1.05f);
            sfxSource.PlayOneShot(clip);
        }
    }
}