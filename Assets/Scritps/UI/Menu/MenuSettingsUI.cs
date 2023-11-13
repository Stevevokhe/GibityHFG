using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettingsUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Slider masterVolumeSlider;
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider sfxVolumeSlider;
    [Header("Other")]
    [SerializeField]
    private AudioSource menuMusicAudioSource;

    private bool isInited = false;
    private float musicVolume = 1f;

    private void Start()
    {
        LoadAllSettings();
    }

    private void Init()
    {
        if (masterVolumeSlider == null)
        {
            throw new ArgumentNullException(nameof(masterVolumeSlider));
        }
        if (musicVolumeSlider == null)
        {
            throw new ArgumentNullException(nameof(musicVolumeSlider));
        }
        if (sfxVolumeSlider == null)
        {
            throw new ArgumentNullException(nameof(sfxVolumeSlider));
        }
        if (menuMusicAudioSource == null)
        {
            throw new ArgumentNullException(nameof(sfxVolumeSlider));
        }
        musicVolume = menuMusicAudioSource.volume;
        isInited = true;
    }

    public void LoadAllSettings()
    {
        if (!isInited)
            Init();

        masterVolumeSlider.value = SavingManager.Instance.GetMasterVolume(1);
        musicVolumeSlider.value = SavingManager.Instance.GetMusicVolume(1);
        menuMusicAudioSource.volume = musicVolumeSlider.value;
        sfxVolumeSlider.value = SavingManager.Instance.GetSFXVolume(1);
    }

    public void SetMasterVolume()
    {
        if (!isInited)
            Init();

        menuMusicAudioSource.volume = musicVolumeSlider.value * masterVolumeSlider.value * musicVolume;

        SavingManager.Instance.SetMasterVolume(masterVolumeSlider.value);
        SavingManager.Instance.Save();
    }

    public void SetMusicVolume()
    {
        if (!isInited)
            Init();

        menuMusicAudioSource.volume = musicVolumeSlider.value * masterVolumeSlider.value * musicVolume;

        SavingManager.Instance.SetMusicVolume(musicVolumeSlider.value);
        SavingManager.Instance.Save();
    }

    public void SetSFXVolume()
    {
        if (!isInited)
            Init();

        SavingManager.Instance.SetSFXVolume(sfxVolumeSlider.value);
        SavingManager.Instance.Save();
    }
}
