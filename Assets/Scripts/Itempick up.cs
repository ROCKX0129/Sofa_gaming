using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemInput : MonoBehaviour
{
    private PlayerInput input;
    private ItemCharacterManager itemManager;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        itemManager = GetComponent<ItemCharacterManager>();
    }

    private void OnEnable()
    {
        input.actions["Using"].performed += OnUsingItem;
    }

    private void OnDisable()
    {
        input.actions["Using"].performed -= OnUsingItem;
    }

    private void OnUsingItem(InputAction.CallbackContext ctx)
    {
        itemManager.CurrentPlayerUsing();
    }
}
