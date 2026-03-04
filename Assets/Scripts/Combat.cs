using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class Combat : MonoBehaviour
{
    [Header("Targeting")]
    public Transform otherPLayer;

    [Header("Weapon")]
    [SerializeField] public SpriteRenderer weaponSprite;
    [SerializeField] private PlayerController playerController;
    public Transform attackPoint;
    public Vector2 attackSize = new Vector2(1.5f, 1f);

    [Header("Attack")]
    public LayerMask attackLayers;
    public float attackCooldown;
    private bool attackPressed;
    private bool canAttack = true;

    [Header("Ranged Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileCooldown;
    private bool canShoot = true;

    [Header("Block")]
    public Collider2D blockCollider;
    public float blockDuration;
    public float blockCooldown;
    private bool canBlock;

    public bool isPlayer2;
    private bool isDead = false;
    public PortalManager portalManager;

   
    [Header("Audio")]
    public AudioClip sfxMelee;          // attack swing / whoosh
    public AudioClip sfxMeleeHit;
    public AudioClip sfxShoot;          // ranged shot
    public AudioClip sfxDeath;          // death sound

    [Header("Audio Volumes")]
    [Range(0f, 1f)] public float meleeVolume = 0.7f;
    [Range(0f, 1f)] public float meleeHitVolume = 0.8f;
    [Range(0f, 1f)] public float shootVolume = 0.7f;
    [Range(0f, 1f)] public float deathVolume = 0.9f;

    private AudioSource sfxSource;

    private void Awake()
    {
        canBlock = true;

        // Configure audio source
        sfxSource = GetComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canAttack)
        {
            attackPressed = true;
        }
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canShoot)
        {
            Debug.Log("Shoot pressed");
            Shoot();
        }
    }

    public void OnBlock(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canBlock)
        {
            canBlock = false;
            if (blockCollider != null)
                blockCollider.enabled = true;
            Debug.Log("Block started");
            Invoke(nameof(EndBlock), blockDuration);
        }
    }

    void FixedUpdate()
    {
        if (attackPressed)
        {
            Debug.Log("attack pressed");
            Attack();
            attackPressed = false;
        }
    }

    void Attack()
    {
        canAttack = false;

        // Play melee swing SFX on attack trigger
        PlayOneShotSafe(sfxMelee, meleeVolume);

        bool hitSomeone = false;

        Collider2D[] hit = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0f, attackLayers);
        foreach (Collider2D playerCol in hit)
        {
            if (playerCol != null && playerCol.transform.root != transform.root)
            {
                Combat targetCombat = playerCol.transform.root.GetComponent<Combat>();
                if (targetCombat != null && targetCombat.blockCollider != null && targetCombat.blockCollider.enabled)
                {
                    Debug.Log("Melee blocked!");
                    continue;
                }

                hitSomeone = true;
                targetCombat.Die();
            }
        }

        //  impact sound (only if we actually hit someone)
        if (hitSomeone)
        {
            PlayOneShotSafe(sfxMeleeHit, meleeHitVolume);
        }

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    // Combat.cs
    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        canShoot = false;

        // Play shoot SFX before/after instantiate (timing preference)
        PlayOneShotSafe(sfxShoot, shootVolume);

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Determine direction based on player facing
        float dirX = playerController.facing; // +1 if right, -1 if left
        proj.GetComponent<Projectile>().SetupDirection(dirX);

        Invoke(nameof(ResetShoot), projectileCooldown);
    }

    void EndBlock()
    {
        if (blockCollider != null)
            blockCollider.enabled = false;
        Debug.Log("Block ended");
        Invoke(nameof(ResetBlock), blockCooldown);
    }

    void ResetAttack() => canAttack = true;
    void ResetShoot() => canShoot = true;
    void ResetBlock() => canBlock = true;

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Death SFX (play once)
        PlayOneShotSafe(sfxDeath, deathVolume);

        GetComponent<PlayerController>().enabled = false;
        enabled = false;
        GetComponent<Animation_connecting_scripts>().PlayDeath();

        if (CompareTag("Player2"))
        {
            PortalManager.Instance.ActivateRightPortal();
        }
        else if (CompareTag("Player1"))
        {
            PortalManager.Instance.ActivateLeftPortal();
        }

        StartCoroutine(DestroyAfterDeath());
    }

    private IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    // --------- Helpers ----------
    private void PlayOneShotSafe(AudioClip clip, float volume)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
    }
}