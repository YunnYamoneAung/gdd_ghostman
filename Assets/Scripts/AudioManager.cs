using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Clips")]
    public AudioClip backgroundMusic;
    public AudioClip pelletSound;
    public AudioClip powerPelletSound;
    public AudioClip deathSound;
    public AudioClip eatGhostSound;

    private AudioSource bgmSource; // looped background music
    private AudioSource sfxSource; // one-shot effects

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Create two separate AudioSources
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        bgmSource.loop = true;
        sfxSource.loop = false;
    }

    private void Start()
    {
        PlayBGM(backgroundMusic);
    }

    // === Background Music ===
    public void PlayBGM(AudioClip clip)
    {
        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // === Sound Effects ===
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // === Stop Everything (BGM + SFX) ===
    public void StopAllSounds()
    {
        bgmSource.Stop();
        sfxSource.Stop();
    }
}
