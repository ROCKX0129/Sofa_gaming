using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUsing : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;
    public static event Action<Vector2> OnUseEvent;
    public static event Action<GameObject> OnPlayerUsing;
    public GameObject telepoterTesting;
    public GameObject PlayerTarget;
    private bool canUsing = true;
    public float ResetCooldown = 3.0f;

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
