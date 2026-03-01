using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUsing : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;
    public event Action<Vector2> OnUseEvent;
    public event Action<GameObject> OnPlayerUsing;
    private GameObject PlayerTarget;
   

    [SerializeField] private ItemCharacterManager itemManager;


    private void Start()
    {
        PlayerTarget = gameObject;
    }
    public void OnUsing(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && itemManager != null)
        {
            // Call the ItemCharacterManager toggle logic
            itemManager.CurrentPlayerUsing();
        }
    }

    private void UsingItem()
    {
        Vector2 myPosition = gameObject.transform.position;
        OnUseEvent?.Invoke(myPosition);
        OnPlayerUsing?.Invoke(PlayerTarget);
    }

 
}
