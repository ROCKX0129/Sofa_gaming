using UnityEngine;
using UnityEngine.InputSystem;

public class COmbat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask attackLayers;
    public float attackRadius;
    public float attackCooldown;

    private bool attackPressed;
    private bool canAttack = true;

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canAttack)
        {
            attackPressed = true;
        }
    }
    void Update()
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

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayers);

        if (hit != null && hit.transform.root != transform.root)
        {
            hit.transform.root.gameObject.SetActive(false);
            Debug.Log(hit.transform.root.name + " was killed ");
        }
    

        Invoke(nameof(ResetAttack), attackCooldown);
    }
    void ResetAttack()
    {
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

}
