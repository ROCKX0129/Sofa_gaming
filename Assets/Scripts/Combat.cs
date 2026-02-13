using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Awake()
    {
        canBlock = true; 
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

                targetCombat.Die();
            }
        }


        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void Shoot()
    {
        

        if (projectilePrefab != null || firePoint != null || otherPLayer != null) return;

        canShoot = false;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        proj.GetComponent<Projectile>().Setup(otherPLayer.position);

        Invoke(nameof(ResetShoot), projectileCooldown);
    }

    void EndBlock()
    {
        if (blockCollider != null)
            blockCollider.enabled = false;

        Debug.Log("Block ended");

        Invoke(nameof(ResetBlock), blockCooldown);
    }
    void ResetAttack()
    {
        canAttack = true;
    }

    void ResetShoot()
    {
        canShoot = true;
    }

    void ResetBlock()
    {
        canBlock = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }

    public void Die()
    {
        gameObject.SetActive(false);
        Debug.Log(transform.root.name + " was killed");
        GameManager.Instance.PlayerDied(gameObject.tag);
    }
}
