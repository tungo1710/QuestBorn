using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    [Header("Background Music")]
    [SerializeField] private AudioClip menuMusicClip;
    [SerializeField] private AudioClip levelMusicClip;

    [Header("Player Clips")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip playerWalkClip;
    [SerializeField] private AudioClip playerAttackClip;
    [SerializeField] private AudioClip playerHurtClip;

    [Header("Enemy Clips")]
    [SerializeField] private AudioClip enemyWalkClip;
    [SerializeField] private AudioClip enemyAttackClip;
    [SerializeField] private AudioClip enemyHurtClip;

    [Header("Other Clips")]
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip healthClip;

    private static AudioManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolumeSettings();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
            PlayMenuMusic();
        else if (scene.name.StartsWith("Level"))
            PlayLevelMusic();
    }

    public bool IsEffectPlaying() => effectAudioSource.isPlaying;

    // Music control
    public void PlayMenuMusic()
    {
        if (backgroundAudioSource.clip != menuMusicClip)
        {
            backgroundAudioSource.Stop();
            backgroundAudioSource.clip = menuMusicClip;
            backgroundAudioSource.loop = true;
            backgroundAudioSource.Play();
        }
    }

    public void PlayLevelMusic()
    {
        if (backgroundAudioSource.clip != levelMusicClip)
        {
            backgroundAudioSource.Stop();
            backgroundAudioSource.clip = levelMusicClip;
            backgroundAudioSource.loop = true;
            backgroundAudioSource.Play();
        }
    }

    // Player sounds
    public void PlayJumpSound() => PlayEffect(jumpClip);
    public void PlayPlayerWalkSound() => PlayEffect(playerWalkClip);
    public void PlayPlayerAttackSound() => PlayEffect(playerAttackClip);
    public void PlayPlayerHurtSound() => PlayEffect(playerHurtClip);

    // Enemy sounds
    public void PlayEnemyWalkSound() => PlayEffect(enemyWalkClip);
    public void PlayEnemyAttackSound() => PlayEffect(enemyAttackClip);
    public void PlayEnemyHurtSound() => PlayEffect(enemyHurtClip);

    // Other
    public void PlayCoinSound() => PlayEffect(coinClip);
    public void PlayHealthSound() => PlayEffect(healthClip);

    private void PlayEffect(AudioClip clip)
    {
        if (clip != null)
        {
            effectAudioSource.PlayOneShot(clip);
        }
    }

    // --- VOLUME CONTROLS ---

    public void SetMusicVolume(float volume)
    {
        backgroundAudioSource.volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("MusicVolume", backgroundAudioSource.volume);
    }

    public void SetSFXVolume(float volume)
    {
        effectAudioSource.volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVolume", effectAudioSource.volume);
    }

    public float GetMusicVolume() => backgroundAudioSource.volume;
    public float GetSFXVolume() => effectAudioSource.volume;

    private void LoadVolumeSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        backgroundAudioSource.volume = musicVolume;
        effectAudioSource.volume = sfxVolume;
    }
}
