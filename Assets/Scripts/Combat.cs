using UnityEngine;
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour
{
    [Header("Weapon")]

    [SerializeField] private float attackOffsetX = 0.6f;
    [SerializeField] public SpriteRenderer weaponSprite;
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
           
            Shoot();
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
                playerCol.transform.root.gameObject.SetActive(false);
                Debug.Log(playerCol.transform.root.name + " was killed ");
            }
        }
        
    

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void Shoot()
    {
        Debug.Log("Shoot pressed");
        Debug.Log("Projectile prefab; " + projectilePrefab);
        if (projectilePrefab != null || firePoint == null) return;

        canShoot = false;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Debug.Log("Projectile Instiantied at: " + proj.transform.position);
        bool facingRight = transform.localScale.x > 0;
        proj.GetComponent<Projectile>().Setup(facingRight);

        Invoke(nameof(ResetShoot), projectileCooldown);
    }
    void ResetAttack()
    {
        canAttack = true;
    }

    void ResetShoot()
    {
        canShoot = true;
    }

    void LateUpdate()
    {
        int facing = transform.localScale.x > 0 ? 1 : -1;

        if (attackPoint != null)
        {
            attackPoint.localPosition = new Vector3(attackOffsetX * facing, attackPoint.localPosition.y, 0);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }

}
