using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public AudioMixer audioMixer;
    public UnityEngine.UI.Slider volumeSlider;
    public UnityEngine.UI.Slider sfxSlider;

    Resolution[] resolutions;

    void Start()
    {
        SetupResolutions();
        SetupQuality();
        LoadSettings();
    }

    void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void SetupQuality()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", index);
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("Quality", index);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }

  public void SetSFXVolume(float volume)
{
    audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    PlayerPrefs.SetFloat("SFXVolume", volume);
}

    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Resolution"))
            SetResolution(PlayerPrefs.GetInt("Resolution"));

        if (PlayerPrefs.HasKey("Quality"))
            SetQuality(PlayerPrefs.GetInt("Quality"));

        if (PlayerPrefs.HasKey("Volume"))
        {
            float volume = PlayerPrefs.GetFloat("Volume");
            volumeSlider.value = volume;
            SetVolume(volume);
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
        float sfx = PlayerPrefs.GetFloat("SFXVolume");
        sfxSlider.value = sfx;
        SetSFXVolume(sfx);
        }
    }
}
