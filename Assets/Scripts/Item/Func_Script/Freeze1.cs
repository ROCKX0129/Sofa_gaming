using UnityEngine;
using System.Collections;

public class Freeze : MonoBehaviour, IItem
{
    [Header("Item Data")]
    public Item_SO itemSO;
    public Item_SO ItemData => itemSO;

    [Header("Projectile Settings")]
    public float speed = 10f;
    public float freezeDuration = 2f;
    public LayerMask playerLayer;

    private bool isShot = false;
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator animator;

    // -------------------------
    //          AUDIO
    // -------------------------
    [Header("Audio (Use SFX)")]
    [Tooltip("Sound to play when the player uses this item (fires it).")]
    public AudioClip sfxUse;
    [Range(0f, 1f)] public float sfxUseVolume = 0.8f;

    // Who used/fired this item (we play the SFX on this object’s AudioSource)
    private Transform owner;
    private AudioSource ownerAudio;  // optional cache
    private AudioSource selfAudio;   // fallback if owner has no AudioSource

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();

        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        if (col == null) col = gameObject.AddComponent<BoxCollider2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        // Prepare a fallback AudioSource on the item (muted until needed)
        selfAudio = GetComponent<AudioSource>();
        if (selfAudio == null) selfAudio = gameObject.AddComponent<AudioSource>();
        selfAudio.playOnAwake = false;
        selfAudio.loop = false;
        selfAudio.spatialBlend = 0f; // 2D sound; set >0 for positional effect
    }

    /// <summary>
    /// Call this immediately after spawning the item, before Shoot().
    /// We’ll try to play the 'use' sound from the owner to keep it local.
    /// </summary>
    public void SetOwner(Transform ownerTransform)
    {
        owner = ownerTransform;
        if (owner != null)
        {
            ownerAudio = owner.GetComponent<AudioSource>();
            // Optionally create one if missing:
            // if (ownerAudio == null) ownerAudio = owner.gameObject.AddComponent<AudioSource>();
            if (ownerAudio != null)
            {
                ownerAudio.playOnAwake = false;
                ownerAudio.loop = false;
                ownerAudio.spatialBlend = 0f; // 2D by default
            }
        }
    }

    public void Shoot(Vector2 direction)
    {
        isShot = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        col.isTrigger = true;
        rb.linearVelocity = direction.normalized * speed;

        // ---- Play owner-only use SFX here ----
        PlayUseSfxAtOwner();
    }

    private void PlayUseSfxAtOwner()
    {
        if (sfxUse == null) return;

        // Prefer playing from the owner (local/owner-only feedback).
        if (ownerAudio != null)
        {
            ownerAudio.PlayOneShot(sfxUse, sfxUseVolume);
        }
        else if (owner != null)
        {
            // Try to find an AudioSource on owner lazily if not cached yet
            var aud = owner.GetComponent<AudioSource>();
            if (aud != null)
            {
                ownerAudio = aud;
                ownerAudio.PlayOneShot(sfxUse, sfxUseVolume);
                return;
            }
            // Fallback: play from the item (audible in world, but still only once on use)
            selfAudio.PlayOneShot(sfxUse, sfxUseVolume);
        }
        else
        {
            // No owner given; fallback to item AudioSource
            selfAudio.PlayOneShot(sfxUse, sfxUseVolume);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isShot) return;

        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            var controller = collision.gameObject.GetComponent<PlayerController>();
            if (controller != null)
                controller.StartCoroutine(controller.Freeze(freezeDuration));
        }

        StartCoroutine(PlayHitAndDestroy());
    }

    private IEnumerator PlayHitAndDestroy()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;

        // We intentionally DO NOT play any sound here,
        // because you asked for "when the player uses it only".
        // If you later want an impact SFX, we can add it here.

        if (animator != null) animator.SetTrigger("hit");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private IEnumerator FreezePlayer(PlayerController controller)
    {
        Debug.Log("Freeze started");
        Rigidbody2D playerRb = controller.GetComponent<Rigidbody2D>();
        controller.isFrozen = true;

        playerRb.linearVelocity = Vector2.zero;
        playerRb.angularVelocity = 0f;
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(freezeDuration);

        Debug.Log("Freeze ended");
        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        controller.isFrozen = false;
    }
}