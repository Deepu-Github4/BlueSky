using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX")]
    public AudioClip cardClick;
    public AudioClip cardFlip;
    public AudioClip correctMatch;
    public AudioClip wrongMatch;
    public AudioClip levelComplete;
    public AudioClip fadeSound;

    [Header("Background Music")]
    public AudioClip bgm;

    private AudioSource sfxSource;
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource.volume = 0.4f;  
    }

    private void Start()
    {
        PlayBGM();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayBGM()
    {
        musicSource.clip = bgm;
        musicSource.Play();
    }
}
