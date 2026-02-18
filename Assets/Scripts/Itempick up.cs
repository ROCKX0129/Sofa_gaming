using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemInput : MonoBehaviour
{
    private PlayerInput playerInput;
    private ItemCharacterManager itemManager;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        itemManager = GetComponent<ItemCharacterManager>();
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
        itemManager.CurrentPlayerUsing();
    }
}
