using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicPlayer : MonoBehaviour
{
    public static BackgroundMusicPlayer Instance { get; private set; }

    [Header("Default BGM Settings")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField][Range(0f, 1f)] private float volume = 0.5f;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private bool dontDestroyOnLoad = true;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = bgmClip;
        _audioSource.volume = volume;
        _audioSource.loop = loop;
        _audioSource.playOnAwake = false;

        if (playOnStart && bgmClip != null)
        {
            _audioSource.Play();
        }
    }

    public void PlayMusic()
    {
        if (!_audioSource.isPlaying && _audioSource.clip != null)
        {
            _audioSource.Play();
        }
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }

    public void TransitionWithStinger(AudioClip stingerClip, AudioClip nextBGM, float stingerDelay = 1.5f, float nextVolume = -1f)
    {
        StartCoroutine(TransitionWithStingerRoutine(stingerClip, nextBGM, stingerDelay, nextVolume));
    }

    private IEnumerator TransitionWithStingerRoutine(AudioClip stingerClip, AudioClip nextBGM, float delay, float nextVolume)
    {
        _audioSource.Stop();
        if (stingerClip != null)
        {
            _audioSource.PlayOneShot(stingerClip);
        }

        yield return new WaitForSecondsRealtime(delay);

        _audioSource.clip = nextBGM;
        _audioSource.loop = true;
        _audioSource.volume = (nextVolume >= 0f) ? nextVolume : volume;
        _audioSource.Play();
    }
}
