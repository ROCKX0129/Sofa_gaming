using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [Header("Footsteps")]
    public AudioClip[] footstepClips;
    public float strideLength = 1.2f;   // world units per step
    public float minSpeedForSteps = 0.05f;

    private AudioSource audioSource;
    private int lastStepIndex = -1;
    private float distanceAcc;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Call this once per frame from PlayerController with the
    // delta distance you consider "foot travel" (usually |dx|)
    public void AccumulateDistance(float deltaX, bool isMovementActive)
    {
        if (!isMovementActive) { distanceAcc = 0f; return; }

        distanceAcc += Mathf.Abs(deltaX);
        while (distanceAcc >= strideLength)
        {
            PlayNextFootstep();
            distanceAcc -= strideLength;
        }
    }

    private void PlayNextFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;

        int index = Random.Range(0, footstepClips.Length);
        while (index == lastStepIndex && footstepClips.Length > 1)
            index = Random.Range(0, footstepClips.Length);

        lastStepIndex = index;
        audioSource.PlayOneShot(footstepClips[index]);
    }
}