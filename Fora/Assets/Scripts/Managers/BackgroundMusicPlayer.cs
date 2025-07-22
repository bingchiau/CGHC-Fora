using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicPlayer : MonoBehaviour
{
    public static BackgroundMusicPlayer Instance { get; private set; }

    [Header("BGM Settings")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField][Range(0f, 1f)] private float volume = 0.5f;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool playOnStart = false; // set to false so it waits for manual trigger
    [SerializeField] private bool dontDestroyOnLoad = true;

    private AudioSource _audioSource;

    private void Awake()
    {
        // Singleton setup
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
}
