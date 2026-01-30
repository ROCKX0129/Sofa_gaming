using UnityEngine;
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour
{
    [SerializeField] SpriteRenderer weaponSprite;

    private PlayerInput playerInput;
    private Rigidbody2D rb;

    private bool attackInput;
    public Transform attackArea;
    public float attackRadius;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        weaponSprite = GetComponent<SpriteRenderer>();
    }
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            attackInput = true;
        Debug.Log("Attack works!" + attackInput);
    }
}
