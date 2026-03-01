using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody2D rb;

    public Vector2 moveInput;
    private bool jumpPressed;

    // Movement
    public float moveSpeed;
    public float jumpForce;

    // Ground Check
    public Transform groundCheck;
    public float groundradius;
    public LayerMask groundLayer;

    private bool isGrounded;

    public int facing = 1;

    public bool isFrozen = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip walkClip;
    public AudioClip jumpClip;
    public float walkSoundInterval = 0.4f;
    private float walkTimer = 0f;

    private SpriteRenderer sprite;
    private Color originalColor;

    private Coroutine freezeRoutine;

    private SpriteRenderer[] cachedRenderers;
    private Color[] cachedOriginalColors;
    void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        sprite = GetComponentInChildren<SpriteRenderer>();
        originalColor = sprite.color;

        cachedRenderers = GetComponentsInChildren<SpriteRenderer>();
        cachedOriginalColors = new Color[cachedRenderers.Length];

        for (int i = 0; i < cachedRenderers.Length; i++)
            cachedOriginalColors[i] = cachedRenderers[i].color;
    }

    private void Update()
    {
        if (moveInput.x != 0)
        {
            facing = (int)Mathf.Sign(moveInput.x);

            if (isGrounded && walkClip != null)
            {
                walkTimer += Time.deltaTime;
                if (walkTimer >= walkSoundInterval)
                {
                    audioSource.PlayOneShot(walkClip);
                    walkTimer = 0f;
                }
            }
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            jumpPressed = true;
    }
    void FixedUpdate()
    {
        if (!isFrozen)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }


        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundradius, groundLayer);

        if (!isFrozen && jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (jumpClip != null && audioSource != null)
                audioSource.PlayOneShot(jumpClip);
        }
        else
        {
            jumpPressed = false;
        }

        if (moveInput.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public IEnumerator Freeze(float duration)
    {
        
        if (freezeRoutine != null)
            StopCoroutine(freezeRoutine);

        freezeRoutine = StartCoroutine(FreezeRoutine(duration));
        yield break;
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        Debug.Log("Freeze started");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        isFrozen = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        Color[] originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].color;

        
        foreach (SpriteRenderer sr in cachedRenderers)
        {
            Color c = sr.color;
            sr.color = new Color(0.4f, 0.7f, 1f, c.a);
        }

        yield return new WaitForSeconds(duration);

        isFrozen = false;

        
        for (int i = 0; i < cachedRenderers.Length; i++)
            cachedRenderers[i].color = cachedOriginalColors[i];

        freezeRoutine = null;
    }


}
