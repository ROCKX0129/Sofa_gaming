using UnityEngine;

/// <summary>
/// Plays randomized footstep sounds with optional pitch variance and
/// a cooldown between steps. Avoids repeating the last clip.
/// </summary>
[DisallowMultipleComponent]
public class FootstepManager : MonoBehaviour
{
    [Header("Audio Source")]
    [Tooltip("If left empty, one will be added to this GameObject.")]
    public AudioSource audioSource;

    [Header("Footstep Clips (3 recommended)")]
    public AudioClip[] footstepClips;

    [Header("Playback")]
    [Tooltip("Seconds between footstep sounds while walking.")]
    public float stepInterval = 0.4f;

    [Range(0f, 1f)]
    public float volume = 0.8f;

    [Header("Pitch Variance")]
    [Tooltip("Random pitch offset applied each step, e.g., 0.05 = ±5%. Set to 0 for no variance.")]
    [Range(0f, 0.25f)]
    public float pitchVariance = 0.05f;

    private float _timer;
    private int _lastIndex = -1;
    private float _basePitch = 1f;

    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure the AudioSource for SFX
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        _basePitch = audioSource.pitch;
    }

    /// <summary>
    /// Call this every frame. When conditions meet (grounded + moving),
    /// it will play a footstep if the interval has elapsed.
    /// </summary>
    /// <param name="isGrounded">Is the character grounded?</param>
    /// <param name="isMovingHorizontally">Is there horizontal movement?</param>
    public void Tick(bool isGrounded, bool isMovingHorizontally)
    {
        if (!isGrounded || !isMovingHorizontally || footstepClips == null || footstepClips.Length == 0)
        {
            // Reset timer slowly so the first step won’t spam if you tap
            _timer = Mathf.Min(_timer, stepInterval);
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= stepInterval)
        {
            PlayRandomFootstep();
            _timer = 0f;
        }
    }

    /// <summary>
    /// Immediately play a step sound (e.g., for animation events). Skips interval.
    /// </summary>
    public void PlayStepImmediate()
    {
        PlayRandomFootstep();
        _timer = 0f;
    }

    private void PlayRandomFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;

        int index;
        if (footstepClips.Length == 1)
        {
            index = 0;
        }
        else
        {
            // pick an index different from last one
            do { index = Random.Range(0, footstepClips.Length); }
            while (index == _lastIndex);
        }

        _lastIndex = index;

        // apply subtle pitch variance
        if (pitchVariance > 0f)
            audioSource.pitch = _basePitch + Random.Range(-pitchVariance, pitchVariance);
        else
            audioSource.pitch = _basePitch;

        audioSource.PlayOneShot(footstepClips[index], volume);
    }
}
