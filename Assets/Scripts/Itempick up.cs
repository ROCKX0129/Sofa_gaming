using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemInput : MonoBehaviour
{
    [SerializeField] private ItemCharacterManager itemManager;
    private PlayerInput playerInput;
    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        itemManager = GetComponentInParent<ItemCharacterManager>();

        if (itemManager == null)
            Debug.LogError(name + " has no ItemCharacterManager assigned!");
    }

    private void OnEnable()
    {
        playerInput.actions["Using"].performed += OnUsingItem;
    }

    private void OnDisable()
    {
        playerInput.actions["Using"].performed -= OnUsingItem;
    }

    private void OnUsingItem(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && itemManager != null)
        {
            itemManager.CurrentPlayerUsing();
        }
    }
}
