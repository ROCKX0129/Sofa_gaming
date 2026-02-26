using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUsing : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;
    public static event Action<Vector2> OnUseEvent;
    public static event Action<GameObject> OnPlayerUsing;
    private GameObject PlayerTarget;
    private bool canUsing = true;
 

    private void Start()
    {
        PlayerTarget = gameObject;
    }
    public void OnUsing(InputAction.CallbackContext ctx)
    {
        
        if (ctx.performed && canUsing)  
        {
            UsingItem();
        }
    }

    private void UsingItem()
    {
        Vector2 myPosition = gameObject.transform.position;
        OnUseEvent?.Invoke(myPosition);
        OnPlayerUsing?.Invoke(PlayerTarget);
    }

 
}
