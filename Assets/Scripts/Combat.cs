using UnityEngine;
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour
{
    [SerializeField] private float attackOffsetX = 0.6f;

    public Transform attackPoint;
    public LayerMask attackLayers;
    public Vector2 attackSize = new Vector2(1.5f, 1f);
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
    void ResetAttack()
    {
        canAttack = true;
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
