using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();

        musicSlider.value = audioManager.GetMusicVolume();
        sfxSlider.value = audioManager.GetSFXVolume();

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        audioManager.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioManager.SetSFXVolume(volume);
    }
}
