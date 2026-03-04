using UnityEngine;
using UnityEngine.Audio;

public class UISoundPlayer : MonoBehaviour
{
    public static UISoundPlayer Instance { get; private set; }

    [Header("Clips")]
    public AudioClip hoverClip;
    public AudioClip clickClip;
    public AudioClip backClip;

    [Header("Audio Source / Mixer")]
    public AudioMixerGroup mixerGroup; // optional but recommended
    private AudioSource _source;

    [Range(0f, 1f)] public float volume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _source = GetComponent<AudioSource>();
        if (_source == null) _source = gameObject.AddComponent<AudioSource>();

        _source.playOnAwake = false;
        _source.loop = false;
        _source.spatialBlend = 0f; // 2D UI sound
        if (mixerGroup != null) _source.outputAudioMixerGroup = mixerGroup;
    }

    public void PlayHover()
    {
        if (hoverClip != null) _source.PlayOneShot(hoverClip, volume);
    }

    public void PlayClick()
    {
        if (clickClip != null) _source.PlayOneShot(clickClip, volume);
    }

    public void PlayBack()
    {
        if (backClip != null) _source.PlayOneShot(backClip, volume);
    }
}
