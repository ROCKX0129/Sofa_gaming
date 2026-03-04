using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;   // currently deactivated until you work on it
    public GameObject controlsPanel;
    public GameObject nameinputPanel;

    [Header("Scene")]
    public string levelSceneName = "level";

    [Header("SFX (assign in Inspector)")]
    public AudioClip sfxClick;         // generic UI click
    public AudioClip sfxBack;          // back / close sound
    public AudioClip sfxConfirm;       // start game / confirm

    [Header("Music (optional)")]
    public AudioClip musicLoop;        // background menu music (optional)
    [Range(0f, 1f)] public float musicVolume = 0.35f;

    [Header("Volumes")]
    [Range(0f, 1f)] public float sfxVolume = 0.7f;

    private AudioSource sfxSource;
    private AudioSource musicSource;

    private void Awake()
    {
        // Ensure clean audio sources
        sfxSource = GetComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;

        // Dedicated music source (optional)
        if (musicLoop != null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = musicLoop;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.playOnAwake = false;
            musicSource.ignoreListenerPause = true; // so menu music keeps playing even if timescale pauses
            musicSource.Play();
        }

        CleanupOtherScenes();
        ShowMainMenu();
    }

    
    private void CleanupOtherScenes()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene != activeScene)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }

    public void ExitGame()
    {
        PlaySfx(sfxBack);
        Application.Quit();
    }

    public void PlayGame()
    {
        PlaySfx(sfxConfirm);
        mainMenuPanel.SetActive(false);
        nameinputPanel.SetActive(true);
        
    }

    public void OpenSettings()
    {
        PlaySfx(sfxClick);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ControlsPanel()
    {
        PlaySfx(sfxClick);
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        PlaySfx(sfxBack);
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        nameinputPanel.SetActive(false);
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
}