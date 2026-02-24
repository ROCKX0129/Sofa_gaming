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

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        qualityDropdown.onValueChanged.AddListener(SetQuality);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
    }

    void SetupQuality()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        qualityDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt("Resolution", index);
        PlayerPrefs.Save();

        resolutionDropdown.value = index;
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);

        PlayerPrefs.SetInt("Quality", index);
        PlayerPrefs.Save();

        qualityDropdown.value = index;
    }

    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Resolution"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("Resolution");
            resolutionDropdown.value = resolutionIndex;
            SetResolution(resolutionIndex);
        }

        if (PlayerPrefs.HasKey("Quality"))
        {
            int qualityIndex = PlayerPrefs.GetInt("Quality");
            qualityDropdown.value = qualityIndex;
            SetQuality(qualityIndex);
        }

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
